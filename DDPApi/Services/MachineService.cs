using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDPApi.Interfaces;
using DDPApi.Models;
using DDPApi.Data;

namespace DDPApi.Services
{
    public class MachineService : IMachine
    {
        private readonly AppDbContext _context;

        public MachineService(AppDbContext context)
        {
            _context = context;
        }

        // Yeni bir makine ekler
        public async Task<Machine> AddMachineAsync(Machine machine)
        {
            machine.CreatedAt = DateTime.UtcNow; // Oluşturma tarihi atanıyor
            await _context.Machines.AddAsync(machine); // Makineyi ekle
            await _context.SaveChangesAsync(); // Değişiklikleri kaydet
            return machine;
        }

        // Verilen ID'ye sahip bir makineyi günceller
        public async Task<Machine> UpdateMachineAsync(int machineId, Machine updatedMachine)
        {
            var machine = await _context.Machines.FindAsync(machineId); // Makineyi bul
            if (machine != null)
            {
                machine.Name = updatedMachine.Name;
                machine.Location = updatedMachine.Location;
                machine.Manufacturer = updatedMachine.Manufacturer;
                machine.Model = updatedMachine.Model;
                machine.PurchaseDate = updatedMachine.PurchaseDate;
                machine.IsOperational = updatedMachine.IsOperational;
                machine.UpdatedAt = DateTime.UtcNow; // Güncelleme tarihi atanıyor

                await _context.SaveChangesAsync(); // Değişiklikleri kaydet
            }

            return machine;
        }

        // Verilen ID'ye sahip bir makineyi siler
        public async Task<bool> DeleteMachineAsync(int machineId)
        {
            var machine = await _context.Machines.FindAsync(machineId); // Makineyi bul
            if (machine != null)
            {
                _context.Machines.Remove(machine); // Makineyi sil
                await _context.SaveChangesAsync(); // Değişiklikleri kaydet
                return true;
            }

            return false;
        }

        // Verilen ID'ye sahip bir makineyi getirir
        public async Task<Machine> GetMachineByIdAsync(int machineId)
        {
            return await _context.Machines.FindAsync(machineId); // Makineyi bul ve döndür
        }

        // Tüm makineleri getirir
        public async Task<IEnumerable<Machine>> GetAllMachinesAsync()
        {
            return await Task.FromResult(_context.Machines.ToList()); // Tüm makineleri döndür
        }
    }
}