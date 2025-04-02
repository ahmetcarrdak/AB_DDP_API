using DDPApi.DTO;
using DDPApi.Interfaces;
using DDPApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using DDPApi.Data;

public class ProductionInstructionService : IProductionInstruction
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly int _companyId;

    public ProductionInstructionService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;

        // JWT'den CompanyId'yi al
        var companyIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("CompanyId");
        if (companyIdClaim != null && int.TryParse(companyIdClaim.Value, out int companyId))
        {
            _companyId = companyId;
        }
    }

    public async Task<ProductionInstruction> CreateProductionInstructionAsync(ProductionInstructionDto instructionDto)
    {
        if (instructionDto == null) throw new ArgumentNullException(nameof(instructionDto));

        var instruction = new ProductionInstruction
        {
            Title = instructionDto.Title,
            Description = instructionDto.Description,
            InsertDate = DateTime.UtcNow,
            CompanyId = _companyId,
            Barcode = instructionDto.Barcode,
            Count = instructionDto.Count
        };

        // ✅ ProductionToMachine tablosuna DTO'dan gelen verileri ekleme
        instruction.ProductionToMachines = instructionDto.ProductionToMachines
            .Select(machine => new ProductionToMachine
            {
                MachineId = machine.MachineId,
                Line = machine.Line,
            }).ToList();

        // ✅ ProductionStore doğrudan model olarak ekleniyor
        instruction.ProductionStores = instructionDto.ProductionStores;

        _context.ProductionInstructions.Add(instruction);
        await _context.SaveChangesAsync();

        return instruction;
    }

    // CompanyId'ye göre tüm üretim talimatlarını ve ilişkili verileri getirme
    public async Task<List<ProductionInstruction>> GetProductionInstructionsByCompanyIdAsync()
    {
        var instructions = await _context.ProductionInstructions
            .Where(pi => pi.CompanyId == _companyId)
            .Include(pi => pi.ProductionToMachines)
            .ThenInclude(ptm => ptm.Machine) // ✅ Machine bilgilerini yükle
            .Include(pi => pi.ProductionStores)
            .Include(pi => pi.ProductToSeans) // ✅ Seans bilgilerini yükle
            .ToListAsync();
        return instructions;
    }

    public async Task<string> ProcessMachineOperation(int machineId, string barcode, int count)
    {
        // 1️⃣ Ana üretim talimatını veya seansı bul
        var production = await _context.ProductionInstructions
            .Include(p => p.ProductionToMachines)
            .Include(p => p.ProductToSeans)
            .FirstOrDefaultAsync(p => p.Barcode == barcode);

        // Üretim talimatı bulunamazsa seans üzerinden arama yap
        if (production == null)
        {
            var seans = await _context.ProductionInstructions
                .Include(p => p.ProductToSeans)
                .FirstOrDefaultAsync(p => p.ProductToSeans.Any(s => s.barcode == barcode));

            if (seans == null)
                return "Üretim talimatı veya seans bulunamadı!";

            production = seans;
        }

        // Üretim adedi kontrolü
        int totalProduced = production.ProductToSeans.Sum(s => s.count);
        if (totalProduced + count > production.Count)
            return "Üretim adedi aşıldı!";

        // 2️⃣ Barkodu okuttuğunda status kontrolü
        var existingBatch = production.ProductToSeans
            .FirstOrDefault(s => s.machineId == machineId && s.barcode == barcode);

        if (existingBatch != null)
        {
            if (existingBatch.status == 2)
            {
                return "Bu makineden çıkış yapılmış!";
            }
            else if (existingBatch.status == 0)
            {
                existingBatch.status = 1;
            }
            else if (existingBatch.status == 1)
            {
                existingBatch.status = 2;
            }

            // Üretim adedini güncelle
            int availableCapacity = existingBatch.BatchSize - existingBatch.count;
            int toAdd = Math.Min(availableCapacity, count);
            existingBatch.count += toAdd;

            if (existingBatch.count == existingBatch.BatchSize)
                existingBatch.isCompleted = true;

            await _context.SaveChangesAsync();
            return "Seans güncellendi!";
        }

        // 3️⃣ Yeni Parti Oluşturma ve Kontrol
        int remaining = count;
        while (remaining > 0)
        {
            var incompleteBatch = production.ProductToSeans
                .FirstOrDefault(s => s.machineId == machineId && s.count < s.BatchSize);

            if (incompleteBatch != null)
            {
                int availableCapacity = incompleteBatch.BatchSize - incompleteBatch.count;
                int toAdd = Math.Min(availableCapacity, remaining);
                incompleteBatch.count += toAdd;
                remaining -= toAdd;

                if (incompleteBatch.count == incompleteBatch.BatchSize)
                    incompleteBatch.isCompleted = true;
            }
            else
            {
                int batchSize = Math.Min(remaining, production.Count - totalProduced);
                var newBatch = new ProductToSeans
                {
                    ProductId = production.Id,
                    count = batchSize,
                    barcode = Guid.NewGuid().ToString("N").Substring(0, 8),
                    machineId = machineId,
                    BatchSize = batchSize,
                    status = 1
                };
                production.ProductToSeans.Add(newBatch);
                remaining -= batchSize;
            }
        }

        // 4️⃣ Üretim tamamlama kontrolü (Tüm makinelerden çıkış yapılmış mı?)
        bool allMachinesExited = production.ProductToSeans
            .All(s => s.status == 2);

        if (allMachinesExited)
        {
            production.isComplated = 1;
            production.ComplatedDate = DateTime.UtcNow;
        }
        else
        {
            production.isComplated = 0;
        }

        await _context.SaveChangesAsync();
        return "success";
    }
}