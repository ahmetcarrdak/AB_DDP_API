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

   public async Task<(bool Success, string Message, ProductionInstruction? Production)> 
        ValidateProduction(string barcode)
    {
        var production = await _context.ProductionInstructions
            .Include(p => p.ProductionToMachines)
                .ThenInclude(pm => pm.Machine)
            .Include(p => p.ProductToSeans)
            .FirstOrDefaultAsync(p => p.Barcode == barcode)
            ?? await _context.ProductionInstructions
                .Include(p => p.ProductToSeans)
                .FirstOrDefaultAsync(p => p.ProductToSeans.Any(s => s.barcode == barcode));

        if (production == null)
            return (false, "Üretim talimatı veya seans bulunamadı!", null);

        if (!production.ProductionToMachines.Any())
            return (false, "Bu üretim için makine ataması yapılmamış!", null);

        return (true, string.Empty, production);
    }

    public async Task<(bool Success, string Message)> 
        CheckPreviousMachines(ProductionInstruction production, int currentMachineId)
    {
        var currentMachine = production.ProductionToMachines
            .FirstOrDefault(pm => pm.MachineId == currentMachineId);
        
        if (currentMachine == null)
            return (false, "Bu makine bu üretim hattında tanımlı değil!");

        var previousMachines = production.ProductionToMachines
            .Where(pm => pm.Line < currentMachine.Line)
            .ToList();

        foreach (var prevMachine in previousMachines)
        {
            var hasSession = production.ProductToSeans
                .Any(s => s.machineId == prevMachine.MachineId);
            
            if (!hasSession)
                return (false, $"{prevMachine.Machine.Name} makinesinden geçiş yapılmamış!");

            var allExited = production.ProductToSeans
                .Where(s => s.machineId == prevMachine.MachineId)
                .All(s => s.status == 2);
            
            if (!allExited)
                return (false, $"{prevMachine.Machine.Name} makinesinden çıkış yapılmamış!");
        }

        return (true, string.Empty);
    }

    public async Task<(bool Success, string Message, ProductToSeans? Session)> 
        GetOrCreateSession(ProductionInstruction production, int machineId, string barcode, int count)
    {
        var existingSession = production.ProductToSeans
            .FirstOrDefault(s => s.machineId == machineId && s.barcode == barcode);

        if (existingSession != null)
        {
            if (existingSession.status == 2)
                return (false, "Bu makineden çıkış yapılmış!", null);

            return (true, string.Empty, existingSession);
        }

        var newSession = new ProductToSeans
        {
            ProductId = production.Id,
            count = count,
            barcode = string.IsNullOrEmpty(barcode) ? Guid.NewGuid().ToString("N").Substring(0, 8) : barcode,
            machineId = machineId,
            BatchSize = production.Count,
            status = 1,
            isCompleted = count >= production.Count
        };

        production.ProductToSeans.Add(newSession);
        return (true, string.Empty, newSession);
    }

    public async Task UpdateMachineStatus(ProductionToMachine machine, int newStatus)
    {
        if (machine.Status == 0 && newStatus == 1)
        {
            machine.EntryDate = DateTime.UtcNow;
        }
        else if (newStatus == 2)
        {
            machine.ExitDate = DateTime.UtcNow;
        }

        machine.Status = newStatus;
    }

    public async Task CheckProductionCompletion(ProductionInstruction production)
    {
        var allMachinesExited = production.ProductionToMachines
            .All(machine => production.ProductToSeans
                .Where(s => s.machineId == machine.MachineId)
                .All(s => s.status == 2));

        production.isComplated = allMachinesExited ? 1 : 0;
        
        if (allMachinesExited)
        {
            production.ComplatedDate = DateTime.UtcNow;
            foreach (var machine in production.ProductionToMachines.Where(m => m.ExitDate == null))
            {
                machine.ExitDate = DateTime.UtcNow;
                machine.Status = 2;
            }
        }
    }
}