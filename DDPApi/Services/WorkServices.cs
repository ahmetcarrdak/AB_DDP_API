using DDPApi.Interfaces;
using DDPApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDPApi.Data;

namespace DDPApi.Services
{
    public class WorkService : IWork
    {
        private readonly AppDbContext _context;

        public WorkService(AppDbContext context)
        {
            _context = context;
        }

        // Tüm işleri getirir
        public async Task<IEnumerable<Work>> GetAllWorksAsync()
        {
            return await _context.Works.ToListAsync();
        }

        // Aktif işleri getirir
        public async Task<IEnumerable<Work>> GetActiveWorksAsync()
        {
            return await _context.Works.Where(w => w.IsActive).ToListAsync();
        }

        // ID'ye göre iş getirir
        public async Task<Work> GetWorkByIdAsync(int id)
        {
            return await _context.Works.FindAsync(id);
        }

        // Yeni iş ekler
        public async Task<bool> AddWorkAsync(Work work)
        {
            try
            {
                _context.Works.Add(work);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // İş bilgilerini günceller
        public async Task<bool> UpdateWorkAsync(Work work)
        {
            try
            {
                _context.Entry(work).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // İş siler
        public async Task<bool> DeleteWorkAsync(int id)
        {
            var work = await _context.Works.FindAsync(id);
            if (work == null)
                return false;

            try
            {
                _context.Works.Remove(work);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Belirli bir departmana ait işleri getirir
        public async Task<IEnumerable<Work>> GetWorksByDepartmentIdAsync(int departmentId)
        {
            return await _context.Works.Where(w => w.AssignedDepartmentId == departmentId).ToListAsync();
        }

        // Belirli bir personele atanmış işleri getirir
        public async Task<IEnumerable<Work>> GetWorksByEmployeeIdAsync(int employeeId)
        {
            return await _context.Works.Where(w => w.AssignedEmployeeId == employeeId).ToListAsync();
        }

        // Öncelik durumuna göre işleri getirir
        public async Task<IEnumerable<Work>> GetWorksByPriorityAsync(string priority)
        {
            return await _context.Works.Where(w => w.Priority == priority).ToListAsync();
        }

        // İş durumuna göre işleri getirir
        public async Task<IEnumerable<Work>> GetWorksByStatusAsync(string status)
        {
            return await _context.Works.Where(w => w.Status == status).ToListAsync();
        }

        // Belirli bir tarih aralığındaki işleri getirir
        public async Task<IEnumerable<Work>> GetWorksByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Works
                .Where(w => w.CreatedDate >= startDate && w.CreatedDate <= endDate)
                .ToListAsync();
        }

        // Gecikmiş işleri getirir
        public async Task<IEnumerable<Work>> GetDelayedWorksAsync()
        {
            var currentDate = DateTime.Now;
            return await _context.Works
                .Where(w => w.DueDate < currentDate && w.CompletionDate == null)
                .ToListAsync();
        }

        // İptal edilmiş işleri getirir
        public async Task<IEnumerable<Work>> GetCancelledWorksAsync()
        {
            return await _context.Works
                .Where(w => !string.IsNullOrEmpty(w.CancellationReason))
                .ToListAsync();
        }

        // Periyodik işleri getirir
        public async Task<IEnumerable<Work>> GetRecurringWorksAsync()
        {
            return await _context.Works.Where(w => w.IsRecurring).ToListAsync();
        }

        // Onay bekleyen işleri getirir
        public async Task<IEnumerable<Work>> GetPendingApprovalWorksAsync()
        {
            return await _context.Works
                .Where(w => w.RequiresApproval && w.ApprovedByEmployeeId == null)
                .ToListAsync();
        }

        // Güvenlik riski olan işleri getirir
        public async Task<IEnumerable<Work>> GetSafetyRiskWorksAsync()
        {
            return await _context.Works.Where(w => w.HasSafetyRisks).ToListAsync();
        }

        // Kalite puanına göre işleri getirir
        public async Task<IEnumerable<Work>> GetWorksByQualityScoreAsync(int minScore)
        {
            return await _context.Works
                .Where(w => w.QualityScore >= minScore)
                .ToListAsync();
        }
    }
}

