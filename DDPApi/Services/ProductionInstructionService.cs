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
    
    public async Task<string> ProcessMachineOperation(int machineId, string barcode)
{
    // 1️⃣ Barkoda göre üretim talimatını bul
    var production = await _context.ProductionInstructions
        .Include(p => p.ProductionToMachines)
        .FirstOrDefaultAsync(p => p.Barcode == barcode);

    if (production == null)
        return "Üretim talimatı bulunamadı!";

    // 2️⃣ Üretim talimatına ait ilgili makine kaydını bul
    var machineProcess = production.ProductionToMachines
        .FirstOrDefault(pm => pm.MachineId == machineId);

    if (machineProcess == null)
        return "Bu makine, üretim sürecinde bulunmuyor!";

    // 3️⃣ Önceki makineler tamamlandı mı?
    var previousMachines = production.ProductionToMachines
        .Where(pm => pm.Line < machineProcess.Line) // Daha önceki makineleri bul
        .Where(pm => pm.ProductionInstructionId == production.Id) // Farklı üretim talimatlarını dışarıda bırak
        .ToList();

    if (previousMachines.Any(pm => pm.Status != 2))
        return "lineError";

    // 4️⃣ Makinede işlem var mı? (Giriş mi çıkış mı belirle)
    if (machineProcess.Status == 0) // İlk defa giriş yapıyorsa
    {
        machineProcess.Status = 1; // Makine sürece başladı
        machineProcess.EntryDate = DateTime.UtcNow;
        production.MachineId = machineId; // Güncel makineyi üretim talimatına ata
    }
    else if (machineProcess.Status == 1) // Makine işlemdeyse çıkış yapacak
    {
        machineProcess.Status = 2; // Çıkış yapıldı
        machineProcess.ExitDate = DateTime.UtcNow;
        

        // Tüm makineler tamamlandı mı?
        bool isLastMachine = production.ProductionToMachines.All(pm => pm.Status == 2);
        if (isLastMachine)
        {
            production.isComplated = 1;
            production.ComplatedDate = DateTime.UtcNow;
        }
    }
    else // Eğer zaten çıkış yapmışsa tekrar giriş engellenir
    {
        return "exitError";
    }

    await _context.SaveChangesAsync();
    return machineProcess.Status == 1 ? "entry" : "exit";
}

}