using System;
using System.Collections.Generic;
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

        public StationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Station>> GetAllStationsAsync()
        {
            return await _context.Stations.ToListAsync();
        }

        public async Task<Station> GetStationByIdAsync(int id)
        {
            return await _context.Stations.FindAsync(id);
        }

        public async Task<Station> CreateStationAsync(Station station)
        {
            _context.Stations.Add(station);
            await _context.SaveChangesAsync();
            return station;
        }

        public async Task<Station> UpdateStationAsync(int id, Station station)
        {
            if (id != station.StationId)
                return null;

            _context.Entry(station).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return station;
        }

        public async Task<bool> DeleteStationAsync(int id)
        {
            var station = await _context.Stations.FindAsync(id);
            if (station == null)
                return false;

            _context.Stations.Remove(station);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Station>> GetActiveStationsAsync()
        {
            return await _context.Stations
                .Where(s => s.IsActive)
                .ToListAsync();
        }

        public async Task<List<Station>> GetStationsByTypeAsync(int stationType)
        {
            return await _context.Stations
                .Where(s => s.StationType == stationType)
                .ToListAsync();
        }

        public async Task<List<Station>> GetStationsByDepartmentAsync(string department)
        {
            return await _context.Stations
                .Where(s => s.Department == department)
                .ToListAsync();
        }

        public async Task<bool> UpdateMaintenanceStatusAsync(int id, bool maintenanceRequired)
        {
            var station = await _context.Stations.FindAsync(id);
            if (station == null)
                return false;

            station.MaintenanceRequired = maintenanceRequired;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Station>> GetStationsRequiringMaintenanceAsync()
        {
            return await _context.Stations
                .Where(s => s.MaintenanceRequired)
                .ToListAsync();
        }

        public async Task<bool> UpdateQualityCheckStatusAsync(int id, bool requiresQualityCheck)
        {
            var station = await _context.Stations.FindAsync(id);
            if (station == null)
                return false;

            station.RequiresQualityCheck = requiresQualityCheck;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Station>> GetStationsByResponsiblePersonAsync(int responsiblePersonId)
        {
            return await _context.Stations
                .Where(s => s.ResponsiblePersonId == responsiblePersonId)
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetTopStationsWithMostPendingJobsAndStagesAsync()
        {
            var stationStats = await _context.Stations
                .Select(s => new
                {
                    Station = s,
                    PendingOrders = _context.Orders.Count(o => o.StationId == s.StationId),
                    PendingWorks = _context.Works.Count(w => w.StationId == s.StationId),
                    TotalPending = _context.Orders.Count(o => o.StationId == s.StationId) + 
                                 _context.Works.Count(w => w.StationId == s.StationId)
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
