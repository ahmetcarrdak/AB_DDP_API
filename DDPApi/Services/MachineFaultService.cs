using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDPApi.Interfaces;
using DDPApi.Models;
using DDPApi.Data;
using Microsoft.EntityFrameworkCore;

namespace DDPApi.Services
{
    public class MachineFaultService : IMachineFault
    {
        private readonly AppDbContext _context;

        public MachineFaultService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MachineFault> AddFaultAsync(MachineFault fault)
        {
            fault.CreatedAt = DateTime.UtcNow; // Oluşturma tarihini ayarla
            await _context.MachineFaults.AddAsync(fault);
            await _context.SaveChangesAsync(); // Veritabanına kaydet
            return fault;
        }

        public async Task<MachineFault> UpdateFaultAsync(int faultId, MachineFault updatedFault)
        {
            var fault = await _context.MachineFaults.FindAsync(faultId);
            if (fault != null)
            {
                fault.MachineId = updatedFault.MachineId;
                fault.FaultDescription = updatedFault.FaultDescription;
                fault.IsResolved = updatedFault.IsResolved;
                fault.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync(); // Güncellemeleri veritabanına kaydet
            }

            return fault;
        }

        public async Task<bool> DeleteFaultAsync(int faultId)
        {
            var fault = await _context.MachineFaults.FindAsync(faultId);
            if (fault != null)
            {
                _context.MachineFaults.Remove(fault);
                await _context.SaveChangesAsync(); // Veritabanından kaldır
                return true;
            }

            return false;
        }

        public async Task<MachineFault> GetFaultByIdAsync(int faultId)
        {
            return await _context.MachineFaults.FindAsync(faultId); // ID'ye göre kayıt getir
        }

        public async Task<IEnumerable<MachineFault>> GetAllFaultsAsync()
        {
            return await _context.MachineFaults
                                 .Include(f => f.Machine)  // Machine ile ilişkiyi dahil ediyoruz
                                 .ToListAsync();  // Asenkron sorguyu çalıştırıyoruz
        }

        public async Task<IEnumerable<MachineFault>> GetUnresolvedFaultsAsync()
        {
            return await Task.FromResult(_context.MachineFaults.Where(f => !f.IsResolved).ToList());
        }

        public async Task<IEnumerable<MachineFault>> GetResolvedFaultsAsync()
        {
            return await Task.FromResult(_context.MachineFaults.Where(f => f.IsResolved).ToList());
        }

        public async Task<IEnumerable<MachineFault>> GetFaultsByMachineCodeAsync(string machineCode)
        {
            return await Task.FromResult(_context.MachineFaults.Where(f => f.MachineCode == machineCode).ToList());
        }

        // İlgili makinedeki toplam arıza sayısını getirir
        public async Task<int> GetTotalFaultCountByMachineIdAsync(int machineId)
        {
            return await _context.MachineFaults.CountAsync(f => f.MachineId == machineId);
        }

        // En çok arıza yapan 5 makineyi getirir
        public async Task<List<Machine>> GetTop5MachinesWithMostFaultsAsync()
        {
            return await _context.Machines
                                 .OrderByDescending(m => m.TotalFault)  // Makinenin toplam arıza sayısına göre sıralar
                                 .Take(5)                               // İlk 5 makineyi alır
                                 .ToListAsync();
        }

        public async Task<List<Machine>> GetLatest5FaultMachinesAsync()
        {
            var latestFaults = await _context.MachineFaults
                                              .OrderByDescending(f => f.CreatedAt)  // En son arızalara göre sırala
                                              .Take(5)                               // İlk 5 kaydı al
                                              .Select(f => f.Machine)                // İlgili makineleri seç
                                              .Distinct()                            // Aynı makineler varsa tekilleştir
                                              .ToListAsync();
            return latestFaults;
        }

    }
}