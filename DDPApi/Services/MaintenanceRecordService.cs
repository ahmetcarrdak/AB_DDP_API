using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DDPApi.Models;
using DDPApi.Data;
using DDPApi.Interfaces;
using System.Linq;

public class MaintenanceRecordService : IMaintenanceRecord
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly int _companyId;

    public MaintenanceRecordService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
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

    public async Task<MaintenanceRecord> AddMaintenanceRecordAsync(MaintenanceRecord maintenanceRecord)
    {
        maintenanceRecord.CompanyId = _companyId;
        _context.MaintenanceRecords.Add(maintenanceRecord);
        await _context.SaveChangesAsync();
        return maintenanceRecord;
    }

    public async Task<MaintenanceRecord> UpdateMaintenanceRecordAsync(int id, MaintenanceRecord maintenanceRecord)
    {
        var existingRecord = await _context.MaintenanceRecords
            .Where(r => r.CompanyId == _companyId && r.MachineId == id)
            .FirstOrDefaultAsync();

        if (existingRecord != null)
        {
            existingRecord.MachineId = maintenanceRecord.MachineId;
            existingRecord.MaintenanceType = maintenanceRecord.MaintenanceType;
            existingRecord.Description = maintenanceRecord.Description;
            existingRecord.MaintenanceDate = maintenanceRecord.MaintenanceDate;
            existingRecord.PerformedBy = maintenanceRecord.PerformedBy;
            existingRecord.Notes = maintenanceRecord.Notes;

            await _context.SaveChangesAsync();
        }

        return existingRecord;
    }

    public async Task<bool> DeleteMaintenanceRecordAsync(int id)
    {
        var record = await _context.MaintenanceRecords
            .Where(r => r.CompanyId == _companyId && r.MachineId == id)
            .FirstOrDefaultAsync();

        if (record != null)
        {
            _context.MaintenanceRecords.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<MaintenanceRecord> GetMaintenanceRecordByIdAsync(int id)
    {
        return await _context.MaintenanceRecords
            .Where(r => r.CompanyId == _companyId && r.MachineId == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<MaintenanceRecord>> GetAllMaintenanceRecordsAsync()
    {
        return await _context.MaintenanceRecords
            .Where(r => r.CompanyId == _companyId)
            .ToListAsync();
    }
}