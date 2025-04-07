using DDPApi.Interfaces;
using DDPApi.Models;
using DDPApi.Data;
using Microsoft.EntityFrameworkCore;

namespace DDPApi.Services
{
    public class MachineFaultService : IMachineFault
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _companyId;

        public MachineFaultService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
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

        public async Task<MachineFault> AddFaultAsync(MachineFault fault)
        {
            // İlgili makineyi veritabanından çekiyoruz
            var machine = await _context.Machines.FindAsync(fault.MachineId);

            if (machine == null || machine.CompanyId != _companyId)
            {
                throw new UnauthorizedAccessException(
                    "Bu şirkete ait olmayan makine için arıza kaydı oluşturamazsınız.");
            }

            // İlişkiyi doğru kurmak için makineyi nesneye set ediyoruz
            fault.Machine = machine;
            fault.CreatedAt = DateTime.UtcNow;
            fault.UpdatedAt = DateTime.UtcNow;

            await _context.MachineFaults.AddAsync(fault);
            await _context.SaveChangesAsync();

            return fault;
        }


        public async Task<MachineFault> UpdateFaultAsync(int faultId, MachineFault updatedFault)
        {
            var fault = await _context.MachineFaults
                .Include(f => f.Machine)
                .FirstOrDefaultAsync(f => f.FaultId == faultId && f.Machine.CompanyId == _companyId);

            if (fault == null)
            {
                return null; // Bu şirkete ait arıza bulunamadı
            }

            // Yeni makine bilgisini doğrula
            if (updatedFault.MachineId != fault.MachineId)
            {
                var newMachine = await _context.Machines.FindAsync(updatedFault.MachineId);
                if (newMachine == null || newMachine.CompanyId != _companyId)
                {
                    throw new UnauthorizedAccessException(
                        "Bu şirkete ait olmayan makine için arıza kaydı güncelleyemezsiniz.");
                }
            }

            fault.MachineId = updatedFault.MachineId;
            fault.FaultDescription = updatedFault.FaultDescription;
            fault.IsResolved = updatedFault.IsResolved;
            fault.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return fault;
        }

        public async Task<bool> DeleteFaultAsync(int faultId)
        {
            var fault = await _context.MachineFaults
                .Include(f => f.Machine)
                .FirstOrDefaultAsync(f => f.FaultId == faultId && f.Machine.CompanyId == _companyId);

            if (fault == null)
            {
                return false; // Bu şirkete ait arıza bulunamadı
            }

            _context.MachineFaults.Remove(fault);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<MachineFault> GetFaultByIdAsync(int faultId)
        {
            return await _context.MachineFaults
                .Include(f => f.Machine)
                .FirstOrDefaultAsync(f => f.FaultId == faultId && f.Machine.CompanyId == _companyId);
        }

        public async Task<IEnumerable<MachineFault>> GetAllFaultsAsync()
        {
            return await _context.MachineFaults
                .Include(f => f.Machine)
                .Where(f => f.Machine.CompanyId == _companyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<MachineFault>> GetUnresolvedFaultsAsync()
        {
            return await _context.MachineFaults
                .Include(f => f.Machine)
                .Where(f => !f.IsResolved && f.Machine.CompanyId == _companyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<MachineFault>> GetResolvedFaultsAsync()
        {
            return await _context.MachineFaults
                .Include(f => f.Machine)
                .Where(f => f.IsResolved && f.Machine.CompanyId == _companyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<MachineFault>> GetFaultsByMachineCodeAsync(string machineCode)
        {
            return await _context.MachineFaults
                .Include(f => f.Machine)
                .Where(f => f.MachineCode == machineCode && f.Machine.CompanyId == _companyId)
                .ToListAsync();
        }

        public async Task<int> GetTotalFaultCountByMachineIdAsync(int machineId)
        {
            // Önce makinenin bu şirkete ait olup olmadığını kontrol et
            var machine = await _context.Machines.FindAsync(machineId);
            if (machine == null || machine.CompanyId != _companyId)
            {
                return 0; // Bu şirkete ait olmayan makine için 0 dön
            }

            return await _context.MachineFaults.CountAsync(f => f.MachineId == machineId);
        }

        public async Task<List<Machine>> GetTop5MachinesWithMostFaultsAsync()
        {
            return await _context.Machines
                .Where(m => m.CompanyId == _companyId)
                .OrderByDescending(m => m.TotalFault)
                .Take(5)
                .ToListAsync();
        }

        public async Task<List<Machine>> GetLatest5FaultMachinesAsync()
        {
            var latestFaults = await _context.MachineFaults
                .Include(f => f.Machine)
                .Where(f => f.Machine.CompanyId == _companyId)
                .OrderByDescending(f => f.CreatedAt)
                .Take(5)
                .Select(f => f.Machine)
                .Distinct()
                .ToListAsync();

            return latestFaults;
        }
    }
}