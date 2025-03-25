using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDPApi.Interfaces;
using DDPApi.Models;
using DDPApi.Data;
using DDPApi.DTO;
using Microsoft.EntityFrameworkCore;

namespace DDPApi.Services
{
    public class MachineService : IMachine
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _companyId;

        public MachineService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
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

        public async Task<Machine> AddMachineAsync(Machine machine)
        {
            machine.CompanyId = _companyId;
            machine.CreatedAt = DateTime.UtcNow;
            await _context.Machines.AddAsync(machine);
            await _context.SaveChangesAsync();
            return machine;
        }

        public async Task<Machine> UpdateMachineAsync(MachineUpdateDto updatedMachineDto)
        {
            var machine = await _context.Machines
                .Where(m => m.CompanyId == updatedMachineDto.CompanyId && m.Id == updatedMachineDto.Id)
                .FirstOrDefaultAsync();

            if (machine != null)
            {
                machine.Name = updatedMachineDto.Name;
                machine.Location = updatedMachineDto.Location;
                machine.Model = updatedMachineDto.Model;
                machine.SerialNumber = updatedMachineDto.SerialNumber;
                machine.Manufacturer = updatedMachineDto.Manufacturer;
                machine.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
            return machine;
        }
        
        public async Task<bool> DeleteMachineAsync(int machineId)
        {
            var machine = await _context.Machines
                .Where(m => m.CompanyId == _companyId && m.Id == machineId)
                .FirstOrDefaultAsync();

            if (machine != null)
            {
                _context.Machines.Remove(machine);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<Machine> GetMachineByIdAsync(int machineId)
        {
            return await _context.Machines
                .Where(m => m.CompanyId == _companyId && m.Id == machineId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Machine>> GetAllMachinesAsync()
        {
            return await _context.Machines
                .Where(m => m.CompanyId == _companyId)
                .ToListAsync();
        }
    }
}