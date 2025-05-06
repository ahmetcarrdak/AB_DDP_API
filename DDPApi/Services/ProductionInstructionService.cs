using DDPApi.DTO;
using DDPApi.Interfaces;
using DDPApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDPApi.Data;
using Microsoft.AspNetCore.Http;

public class ProductionInstructionService : IProductionInstruction
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly int _companyId;

    public ProductionInstructionService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _companyId = GetCompanyIdFromToken();
    }

    private int GetCompanyIdFromToken()
    {
        var companyIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("CompanyId");
        return companyIdClaim != null && int.TryParse(companyIdClaim.Value, out int companyId) 
            ? companyId 
            : throw new UnauthorizedAccessException("Geçersiz şirket kimliği");
    }

    public async Task<ProductionInstruction> CreateProductionInstructionAsync(ProductionInstructionDto instructionDto)
    {
        if (instructionDto == null) 
            throw new ArgumentNullException(nameof(instructionDto));

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var instruction = new ProductionInstruction
            {
                Title = instructionDto.Title,
                Description = instructionDto.Description,
                InsertDate = DateTime.UtcNow,
                CompanyId = _companyId,
                Barcode = instructionDto.Barcode ?? Guid.NewGuid().ToString("N").Substring(0, 10),
                Count = instructionDto.Count,
                ProductionToMachines = instructionDto.ProductionToMachines?
                    .Select(m => new ProductionToMachine
                    {
                        MachineId = m.MachineId,
                        Line = m.Line,
                        Status = 0,
                        EntryDate = null,
                        ExitDate = null
                    }).ToList() ?? new List<ProductionToMachine>(),
                ProductionStores = instructionDto.ProductionStores ?? new List<ProductionStore>()
            };

            await _context.ProductionInstructions.AddAsync(instruction);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return instruction;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<ProductionInstruction>> GetProductionInstructionsByCompanyIdAsync()
    {
        return await _context.ProductionInstructions
            .Where(pi => pi.CompanyId == _companyId && pi.isDeleted == 0)
            .Include(pi => pi.ProductionToMachines)
                .ThenInclude(ptm => ptm.Machine)
            .Include(pi => pi.ProductionStores)
            .Include(pi => pi.ProductToSeans)
            .OrderByDescending(pi => pi.InsertDate)
            .ToListAsync();
    }

    public async Task<(bool Success, string Message, ProductionInstruction? Production)> 
        ValidateProduction(string barcode)
    {
        var production = await _context.ProductionInstructions
            .Include(p => p.ProductionToMachines)
                .ThenInclude(pm => pm.Machine)
            .Include(p => p.ProductToSeans)
            .FirstOrDefaultAsync(p => p.Barcode == barcode && p.CompanyId == _companyId)
            ?? await _context.ProductionInstructions
                .Include(p => p.ProductToSeans)
                .FirstOrDefaultAsync(p => p.ProductToSeans.Any(s => s.barcode == barcode) && p.CompanyId == _companyId);

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
            .OrderBy(pm => pm.Line)
            .ToList();

        foreach (var prevMachine in previousMachines)
        {
            var sessions = production.ProductToSeans
                .Where(s => s.machineId == prevMachine.MachineId)
                .ToList();

            if (!sessions.Any())
                return (false, $"{prevMachine.Machine.Name} makinesinden geçiş yapılmamış!");

            if (!sessions.All(s => s.status == 2))
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
            isCompleted = count >= production.Count,
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