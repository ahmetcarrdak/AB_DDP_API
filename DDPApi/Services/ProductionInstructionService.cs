using DDPApi.DTO;
using DDPApi.Interfaces;
using DDPApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDPApi.Data;

public class ProductionInstructionService : IProductionInstruction
{
    private readonly AppDbContext _context;
    private readonly int _companyId;
    private readonly IHttpContextAccessor _httpContextAccessor;

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
        var production = new ProductionInstruction
        {
            Barcode = instructionDto.Barcode,
            Count = instructionDto.Count,
            Title = instructionDto.Title,
            Description = instructionDto.Description,
            InsertDate = DateTime.UtcNow,
            ProductionToMachines = instructionDto.ProductionToMachines?
                .Select(m => new ProductionToMachine
                {
                    MachineId = m.MachineId,
                    Line = m.Line,
                    Status = 0,
                    EntryDate = null,
                    ExitDate = null
                }).ToList() ?? new List<ProductionToMachine>(),
            CompanyId = _companyId,
        };

        await _context.ProductionInstructions.AddAsync(production);
        await _context.SaveChangesAsync();
        return production;
    }

    public async Task<List<ProductionInstruction>> GetProductionInstructionsByCompanyIdAsync()
    {
        return await _context.ProductionInstructions
            .Where(p => p.CompanyId == _companyId)
            .Include(p => p.ProductionToMachines)
            .ThenInclude(m => m.Machine)
            .Include(p => p.ProductToSeans)
            .OrderByDescending(p => p.InsertDate)
            .ToListAsync();
    }

    public async Task<(bool Success, string Message, ProductionInstruction? Production)> ValidateProduction(string barcode)
    {
        var production = await _context.ProductionInstructions
            .Include(p => p.ProductionToMachines)
            .ThenInclude(m => m.Machine)
            .Include(p => p.ProductToSeans)
            .FirstOrDefaultAsync(p => p.Barcode == barcode);

        if (production == null) 
            return (false, "Üretim talimatı bulunamadı", null);

        return (true, string.Empty, production);
    }

    public async Task<(bool Success, string Message)> CheckPreviousMachines(ProductionInstruction production, int currentMachineId)
    {
        var currentMachine = production.ProductionToMachines.FirstOrDefault(m => m.MachineId == currentMachineId);
        if (currentMachine == null)
            return (false, "Mevcut makine bulunamadı");

        var previousMachines = production.ProductionToMachines
            .Where(m => m.Line < currentMachine.Line)
            .OrderBy(m => m.Line);

        foreach (var machine in previousMachines)
        {
            if (!production.ProductToSeans.Any(s => s.machineId == machine.MachineId))
                return (false, $"{machine.Machine.Name} makinesi atlanmış");
        }

        return (true, string.Empty);
    }

    public async Task<(bool Success, string Message, ProductToSeans? Session)> GetOrCreateSession(
        ProductionInstruction production, int machineId, string barcode, int count)
    {
        var existingSession = production.ProductToSeans
            .FirstOrDefault(s => s.machineId == machineId && s.barcode == barcode);

        if (existingSession != null)
        {
            if (existingSession.status == 2)
                return (false, "Bu seans zaten tamamlanmış", null);

            return (true, string.Empty, existingSession);
        }

        var newSession = new ProductToSeans
        {
            ProductId = production.Id,
            machineId = machineId,
            barcode = barcode,
            count = count,
            status = 0
        };

        production.ProductToSeans.Add(newSession);
        return (true, string.Empty, newSession);
    }

    public async Task UpdateMachineStatus(ProductionToMachine machine, int newStatus)
    {
        machine.Status = newStatus;
        if (newStatus == 1) machine.EntryDate = DateTime.UtcNow;
        if (newStatus == 2) machine.ExitDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task CheckProductionCompletion(ProductionInstruction production)
    {
        var allCompleted = production.ProductionToMachines.All(machine => 
            production.ProductToSeans.Any(s => 
                s.machineId == machine.MachineId && 
                s.status == 2));

        production.isComplated = allCompleted ? 1 : 0;
        await _context.SaveChangesAsync();
    }
}