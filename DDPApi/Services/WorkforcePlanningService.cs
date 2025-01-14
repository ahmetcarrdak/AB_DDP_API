using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DDPApi.Models;
using DDPApi.Data;
using DDPApi.Interfaces;

public class WorkforcePlanningService : IWorkforcePlanning
{
    private readonly AppDbContext _context;

    public WorkforcePlanningService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<WorkforcePlanning> AddWorkforcePlanningAsync(WorkforcePlanning workforcePlanning)
    {
        _context.WorkforcePlannings.Add(workforcePlanning);
        await _context.SaveChangesAsync();
        return workforcePlanning;
    }

    public async Task<WorkforcePlanning> UpdateWorkforcePlanningAsync(int id, WorkforcePlanning workforcePlanning)
    {
        var existingPlanning = await _context.WorkforcePlannings.FindAsync(id);
        if (existingPlanning != null)
        {
            existingPlanning.EmployeeId = workforcePlanning.EmployeeId;
            existingPlanning.StationId = workforcePlanning.StationId;
            existingPlanning.ShiftStart = workforcePlanning.ShiftStart;
            existingPlanning.ShiftEnd = workforcePlanning.ShiftEnd;
            existingPlanning.TaskDescription = workforcePlanning.TaskDescription;
            existingPlanning.IsAssigned = workforcePlanning.IsAssigned;

            await _context.SaveChangesAsync();
        }
        return existingPlanning;
    }

    public async Task<bool> DeleteWorkforcePlanningAsync(int id)
    {
        var planning = await _context.WorkforcePlannings.FindAsync(id);
        if (planning != null)
        {
            _context.WorkforcePlannings.Remove(planning);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<WorkforcePlanning> GetWorkforcePlanningByIdAsync(int id)
    {
        return await _context.WorkforcePlannings.FindAsync(id);
    }

    public async Task<IEnumerable<WorkforcePlanning>> GetAllWorkforcePlanningsAsync()
    {
        return await _context.WorkforcePlannings.ToListAsync();
    }
}
