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
        // 1️⃣ Üretim talimatını veya seansı bul
        var production = await _context.ProductionInstructions
                             .Include(p => p.ProductionToMachines)
                             .ThenInclude(pm => pm.Machine)
                             .Include(p => p.ProductToSeans)
                             .FirstOrDefaultAsync(p => p.Barcode == barcode)
                         ?? await _context.ProductionInstructions
                             .Include(p => p.ProductToSeans)
                             .FirstOrDefaultAsync(p => p.ProductToSeans.Any(s => s.barcode == barcode));

        if (production == null)
            return "Üretim talimatı veya seans bulunamadı!";

        // 2️⃣ Makine atanmış mı kontrol et
        if (!production.ProductionToMachines.Any())
            return "Bu üretim için makine ataması yapılmamış!";

        // 3️⃣ İşlem yapılan makinenin üretim sırasındaki konumunu bul
        var currentMachine = production.ProductionToMachines
            .FirstOrDefault(pm => pm.MachineId == machineId);

        if (currentMachine == null)
            return "Bu makine bu üretim hattında tanımlı değil!";

        // 4️⃣ Önceki makinelerden çıkış yapılmış mı kontrol et
        var previousMachines = production.ProductionToMachines
            .Where(pm => pm.Line < currentMachine.Line)
            .ToList();

        foreach (var prevMachine in previousMachines)
        {
            var prevMachineSessions = production.ProductToSeans
                .Where(s => s.machineId == prevMachine.MachineId)
                .ToList();

            if (!prevMachineSessions.Any())
                return $"{prevMachine.Machine.Name} makinesinden geçiş yapılmamış!";

            if (prevMachineSessions.Any(s => s.status != 2))
                return $"{prevMachine.Machine.Name} makinesinden çıkış yapılmamış!";
        }

        // 5️⃣ Mevcut makinedeki seansı bul veya oluştur
        var existingSession = production.ProductToSeans
            .FirstOrDefault(s => s.machineId == machineId && s.barcode == barcode);

        if (existingSession != null)
        {
            // Çıkış yapılmışsa işleme izin verme
            if (existingSession.status == 2)
                return "Bu makineden çıkış yapılmış!";

            // Giriş yapılmamışsa giriş yap
            if (existingSession.status == 0)
                existingSession.status = 1;

            // Adet güncelleme
            existingSession.count += count;

            // Batch boyutuna ulaştıysa tamamlandı olarak işaretle
            if (existingSession.count >= existingSession.BatchSize)
                existingSession.isCompleted = true;

            await _context.SaveChangesAsync();
            return "Seans güncellendi!";
        }

        // 6️⃣ Yeni seans oluştur
        var newSession = new ProductToSeans
        {
            ProductId = production.Id,
            count = count,
            barcode = string.IsNullOrEmpty(barcode) ? Guid.NewGuid().ToString("N").Substring(0, 8) : barcode,
            machineId = machineId,
            BatchSize = production.Count, // Varsayılan batch boyutu
            status = 1, // Giriş yapıldı olarak işaretle
            isCompleted = count >= production.Count
        };

        production.ProductToSeans.Add(newSession);

        // 7️⃣ Makine giriş tarihini güncelle (ilk girişse)
        if (currentMachine.Status == 0)
        {
            currentMachine.Status = 1;
            currentMachine.EntryDate = DateTime.UtcNow;
        }

        // 8️⃣ Tüm makinelerden geçiş yapıldı mı kontrol et
        var allMachines = production.ProductionToMachines.OrderBy(pm => pm.Line).ToList();
        bool allCompleted = true;

        foreach (var machine in allMachines)
        {
            var sessions = production.ProductToSeans
                .Where(s => s.machineId == machine.MachineId)
                .ToList();

            if (!sessions.Any() || sessions.Any(s => s.status != 2))
            {
                allCompleted = false;
                break;
            }
        }

        // 9️⃣ Üretim tamamlandıysa işaretle
        if (allCompleted)
        {
            production.isComplated = 1;
            production.ComplatedDate = DateTime.UtcNow;

            // Tüm makinelerin çıkış tarihini güncelle
            foreach (var machine in allMachines.Where(m => m.ExitDate == null))
            {
                machine.ExitDate = DateTime.UtcNow;
                machine.Status = 2;
            }
        }
        else
        {
            production.isComplated = 0;
        }

        await _context.SaveChangesAsync();
        return "success";
    }
}