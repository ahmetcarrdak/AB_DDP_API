using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDPApi.Data;
using DDPApi.Interfaces;
using DDPApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DDPApi.Services
{
    public class StationService : IStation
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _companyId;

        public StationService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
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

        public async Task<List<Station>> GetAllStationsAsync()
        {
            return await _context.Stations
                .Where(s => s.CompanyId == _companyId)
                .ToListAsync();
        }

        public async Task<Station> GetStationByIdAsync(int id)
        {
            return await _context.Stations
                .Where(s => s.CompanyId == _companyId && s.StationId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Station> CreateStationAsync(Station station)
        {
            station.CompanyId = _companyId;
            _context.Stations.Add(station);
            await _context.SaveChangesAsync();
            return station;
        }

        public async Task<Station> UpdateStationAsync(int id, Station station)
        {
            var existingStation = await _context.Stations
                .Where(s => s.CompanyId == _companyId && s.StationId == id)
                .FirstOrDefaultAsync();

            if (existingStation == null)
                return null;

            existingStation.Name = station.Name;
            existingStation.Description = station.Description;
            existingStation.StationType = station.StationType;
            existingStation.Department = station.Department;
            existingStation.IsActive = station.IsActive;
            existingStation.MaintenanceRequired = station.MaintenanceRequired;
            existingStation.RequiresQualityCheck = station.RequiresQualityCheck;
            existingStation.ResponsiblePersonId = station.ResponsiblePersonId;

            await _context.SaveChangesAsync();
            return existingStation;
        }

        public async Task<bool> DeleteStationAsync(int id)
        {
            var station = await _context.Stations
                .Where(s => s.CompanyId == _companyId && s.StationId == id)
                .FirstOrDefaultAsync();

            if (station == null)
                return false;

            _context.Stations.Remove(station);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Station>> GetActiveStationsAsync()
        {
            return await _context.Stations
                .Where(s => s.CompanyId == _companyId && s.IsActive)
                .ToListAsync();
        }

        public async Task<List<Station>> GetStationsByTypeAsync(int stationType)
        {
            return await _context.Stations
                .Where(s => s.CompanyId == _companyId && s.StationType == stationType)
                .ToListAsync();
        }

        public async Task<List<Station>> GetStationsByDepartmentAsync(string department)
        {
            return await _context.Stations
                .Where(s => s.CompanyId == _companyId && s.Department == department)
                .ToListAsync();
        }

        public async Task<bool> UpdateMaintenanceStatusAsync(int id, bool maintenanceRequired)
        {
            var station = await _context.Stations
                .Where(s => s.CompanyId == _companyId && s.StationId == id)
                .FirstOrDefaultAsync();

            if (station == null)
                return false;

            station.MaintenanceRequired = maintenanceRequired;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Station>> GetStationsRequiringMaintenanceAsync()
        {
            return await _context.Stations
                .Where(s => s.CompanyId == _companyId && s.MaintenanceRequired)
                .ToListAsync();
        }

        public async Task<bool> UpdateQualityCheckStatusAsync(int id, bool requiresQualityCheck)
        {
            var station = await _context.Stations
                .Where(s => s.CompanyId == _companyId && s.StationId == id)
                .FirstOrDefaultAsync();

            if (station == null)
                return false;

            station.RequiresQualityCheck = requiresQualityCheck;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Station>> GetStationsByResponsiblePersonAsync(int responsiblePersonId)
        {
            return await _context.Stations
                .Where(s => s.CompanyId == _companyId && s.ResponsiblePersonId == responsiblePersonId)
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetTopStationsWithMostPendingJobsAndStagesAsync()
        {
            var stationStats = await _context.Stations
                .Where(s => s.CompanyId == _companyId)
                .Select(s => new
                {
                    Station = s,
                    PendingOrders = _context.Orders.Count(o => o.CompanyId == _companyId && o.StationId == s.StationId),
                    PendingWorks = _context.Works.Count(w => w.CompanyId == _companyId && w.StationId == s.StationId),
                    TotalPending = _context.Orders.Count(o => o.CompanyId == _companyId && o.StationId == s.StationId) + 
                                 _context.Works.Count(w => w.CompanyId == _companyId && w.StationId == s.StationId)
                })
                .OrderByDescending(x => x.TotalPending)
                .Take(5)
                .Select(x => new
                {
                    StationId = x.Station.StationId,
                    StationName = x.Station.Name,
                    PendingOrders = x.PendingOrders,
                    PendingWorks = x.PendingWorks,
                    TotalPendingItems = x.TotalPending
                })
                .ToListAsync();

            return stationStats;
        }
    }
}
