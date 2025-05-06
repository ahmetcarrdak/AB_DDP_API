using DDPApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using DDPApi.Data;

public class ProductionInstructionService
{
    private readonly AppDbContext _context;

    public ProductionInstructionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string> ProcessMachineOperation(int machineId, string barcode, int count)
    {
        // 1. Üretim talimatını bul
        var production = await _context.ProductionInstructions
            .Include(p => p.ProductionToMachines)
            .Include(p => p.ProductToSeans)
            .FirstOrDefaultAsync(p => p.Barcode == barcode);

        if (production == null)
            return "Üretim talimatı bulunamadı";

        // 2. Üretim sınır kontrolü
        var totalProduced = production.ProductToSeans.Sum(s => s.count);
        if (totalProduced + count > production.Count)
            return "Üretim sınırı aşıldı";

        // 3. Makine kontrolü
        var currentMachine = production.ProductionToMachines
            .FirstOrDefault(pm => pm.MachineId == machineId);

        if (currentMachine == null)
            return "Bu makine üretim hattında değil";

        // 4. Önceki makineler kontrolü
        var previousMachines = production.ProductionToMachines
            .Where(pm => pm.Line < currentMachine.Line)
            .OrderBy(pm => pm.Line)
            .ToList();

        foreach (var prevMachine in previousMachines)
        {
            var hasSession = production.ProductToSeans
                .Any(s => s.machineId == prevMachine.MachineId);

            if (!hasSession)
                return $"makinesinden geçiş yapılmamış";
        }

        // 5. Mevcut seansı bul veya oluştur
        var session = production.ProductToSeans
            .FirstOrDefault(s => s.machineId == machineId && s.barcode == barcode);

        if (session == null)
        {
            session = new ProductToSeans
            {
                ProductId = production.Id,
                machineId = machineId,
                barcode = barcode,
                count = count,
                status = 1
            };
            production.ProductToSeans.Add(session);
        }
        else
        {
            session.count += count;
        }

        // 6. Makine durumunu güncelle
        if (currentMachine.Status == 0)
        {
            currentMachine.Status = 1;
            currentMachine.EntryDate = DateTime.Now;
        }

        await _context.SaveChangesAsync();
        return "İşlem başarılı";
    }

    public async Task<string> ExitMachine(int machineId, string barcode)
    {
        // 1. Üretim talimatını bul
        var production = await _context.ProductionInstructions
            .Include(p => p.ProductionToMachines)
            .Include(p => p.ProductToSeans)
            .FirstOrDefaultAsync(p => p.ProductToSeans.Any(s => s.barcode == barcode));

        if (production == null)
            return "Üretim talimatı bulunamadı";

        // 2. Seansı bul
        var session = production.ProductToSeans
            .FirstOrDefault(s => s.machineId == machineId && s.barcode == barcode);

        if (session == null)
            return "Seans bulunamadı";

        // 3. Makineyi bul
        var machine = production.ProductionToMachines
            .FirstOrDefault(pm => pm.MachineId == machineId);

        if (machine == null)
            return "Makine bulunamadı";

        // 4. Çıkış işlemi
        session.status = 2;
        machine.Status = 2;
        machine.ExitDate = DateTime.Now;

        await _context.SaveChangesAsync();
        return "Çıkış işlemi başarılı";
    }
}