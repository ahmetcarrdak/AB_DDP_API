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
            CompanyId = _companyId
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
            .ThenInclude(ptm => ptm.Machine)  // ✅ Machine bilgilerini yükle
            .Include(pi => pi.ProductionStores)  
            .ToListAsync();
        return instructions;
    }
}