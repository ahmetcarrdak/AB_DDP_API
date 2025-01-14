using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DDPApi.Models;
using DDPApi.Data;
using DDPApi.Interfaces;

public class MaintenanceRecordService : IMaintenanceRecord
{
    private readonly AppDbContext _context;

    public MaintenanceRecordService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<MaintenanceRecord> AddMaintenanceRecordAsync(MaintenanceRecord maintenanceRecord)
    {
        _context.MaintenanceRecords.Add(maintenanceRecord);
        await _context.SaveChangesAsync();
        return maintenanceRecord;
    }

    public async Task<MaintenanceRecord> UpdateMaintenanceRecordAsync(int id, MaintenanceRecord maintenanceRecord)
    {
        var existingRecord = await _context.MaintenanceRecords.FindAsync(id);
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
        var record = await _context.MaintenanceRecords.FindAsync(id);
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
        return await _context.MaintenanceRecords.FindAsync(id);
    }

    public async Task<IEnumerable<MaintenanceRecord>> GetAllMaintenanceRecordsAsync()
    {
        return await _context.MaintenanceRecords.ToListAsync();
    }
}