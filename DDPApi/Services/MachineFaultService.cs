using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDPApi.Interfaces;
using DDPApi.Models;
using DDPApi.Data;

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
            return await Task.FromResult(_context.MachineFaults.ToList());
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
    }
}