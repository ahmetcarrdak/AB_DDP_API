using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DDPApi.Models;
using DDPApi.Data;
using DDPApi.Interfaces;

public class AlertService : IAlert
{
    private readonly AppDbContext _context;

    public AlertService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Alert> AddAlertAsync(Alert alert)
    {
        _context.Alerts.Add(alert);
        await _context.SaveChangesAsync();
        return alert;
    }

    public async Task<Alert> UpdateAlertAsync(int id, Alert alert)
    {
        var existingAlert = await _context.Alerts.FindAsync(id);
        if (existingAlert != null)
        {
            existingAlert.Message = alert.Message;
            existingAlert.AlertType = alert.AlertType;
            existingAlert.IsResolved = alert.IsResolved;
            existingAlert.AlertDate = alert.AlertDate;

            await _context.SaveChangesAsync();
        }
        return existingAlert;
    }

    public async Task<bool> DeleteAlertAsync(int id)
    {
        var alert = await _context.Alerts.FindAsync(id);
        if (alert != null)
        {
            _context.Alerts.Remove(alert);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<Alert> GetAlertByIdAsync(int id)
    {
        return await _context.Alerts.FindAsync(id);
    }

    public async Task<IEnumerable<Alert>> GetAllAlertsAsync()
    {
        return await _context.Alerts.ToListAsync();
    }

    public async Task<IEnumerable<Alert>> GetUnresolvedAlertsAsync()
    {
        return await _context.Alerts.Where(a => !a.IsResolved).ToListAsync();
    }

    public async Task<IEnumerable<Alert>> GetResolvedAlertsAsync()
    {
        return await _context.Alerts.Where(a => a.IsResolved).ToListAsync();
    }
}