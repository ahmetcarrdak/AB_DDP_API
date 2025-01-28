using DDPApi.Interfaces;
using DDPApi.Models;
using DDPApi.DTO;
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
            return await _context.Works.AsNoTracking().ToListAsync();
        }

        // Aktif işleri getirir
        public async Task<IEnumerable<WorkDto>> GetActiveWorksAsync()
        {
            return await _context.Works
                .Where(w => w.IsActive)
                .Select(w => MapToDto(w))
                .ToListAsync();
        }

        // ID'ye göre iş getirir
        public async Task<WorkDto> GetWorkByIdAsync(int id)
        {
            var work = await _context.Works.FindAsync(id);
            return work != null ? MapToDto(work) : null;
        }

        // Yeni iş ekler
        public async Task<bool> AddWorkAsync(WorkDto workDto)
        {
            try
            {
                var work = MapToModel(workDto);
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
        public async Task<bool> UpdateWorkAsync(WorkDto workDto)
        {
            var existingWork = await _context.Works.FindAsync(workDto.WorkId);
            if (existingWork == null)
            {
                return false;
            }

            try
            {
                // Mevcut işin alanlarını DTO'dan güncelle
                existingWork.WorkName = workDto.WorkName;
                existingWork.Description = workDto.Description;
                existingWork.Status = workDto.Status;
                existingWork.Priority = workDto.Priority;
                existingWork.StartDate = workDto.StartDate;
                existingWork.DueDate = workDto.DueDate;
                existingWork.CompletionDate = workDto.CompletionDate;
                existingWork.Location = workDto.Location;
                existingWork.EstimatedCost = workDto.EstimatedCost;
                existingWork.ActualCost = workDto.ActualCost;
                existingWork.EstimatedDuration = workDto.EstimatedDuration;
                existingWork.ActualDuration = workDto.ActualDuration;
                existingWork.RequiredEquipment = workDto.RequiredEquipment;
                existingWork.RequiredMaterials = workDto.RequiredMaterials;
                existingWork.IsRecurring = workDto.IsRecurring;
                existingWork.RecurrencePattern = workDto.RecurrencePattern;
                existingWork.RequiresApproval = workDto.RequiresApproval;
                existingWork.Notes = workDto.Notes;
                existingWork.IsActive = workDto.IsActive;
                existingWork.CancellationReason = workDto.CancellationReason;
                existingWork.CancellationDate = workDto.CancellationDate;
                existingWork.QualityScore = workDto.QualityScore;
                existingWork.QualityNotes = workDto.QualityNotes;
                existingWork.HasSafetyRisks = workDto.HasSafetyRisks;
                existingWork.SafetyNotes = workDto.SafetyNotes;

                _context.Works.Update(existingWork);
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

        // Diğer filtreleme metodları DTO dönüşümü ile güncellendi
        public async Task<IEnumerable<WorkDto>> GetWorksByDepartmentIdAsync(int departmentId)
        {
            return await _context.Works
                .Select(w => MapToDto(w))
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkDto>> GetWorksByEmployeeIdAsync(int employeeId)
        {
            return await _context.Works
                .Where(w => w.AssignedEmployeeId == employeeId)
                .Select(w => MapToDto(w))
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkDto>> GetWorksByPriorityAsync(string priority)
        {
            return await _context.Works
                .Where(w => w.Priority == priority)
                .Select(w => MapToDto(w))
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkDto>> GetWorksByStatusAsync(string status)
        {
            return await _context.Works
                .Where(w => w.Status == status)
                .Select(w => MapToDto(w))
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkDto>> GetWorksByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Works
                .Where(w => w.CreatedDate >= startDate && w.CreatedDate <= endDate)
                .Select(w => MapToDto(w))
                .ToListAsync();
        }

        // İptal edilmiş işleri getirir
        public async Task<IEnumerable<WorkDto>> GetCancelledWorksAsync()
        {
            return await _context.Works
                .Where(w => !string.IsNullOrEmpty(w.CancellationReason))
                .Select(w => MapToDto(w))
                .ToListAsync();
        }

        // Gecikmiş işleri getirir
        public async Task<IEnumerable<WorkDto>> GetDelayedWorksAsync()
        {
            var currentDate = DateTime.Now;
            return await _context.Works
                .Where(w => w.DueDate < currentDate && w.CompletionDate == null)
                .Select(w => MapToDto(w))
                .ToListAsync();
        }

        // Onay bekleyen işleri getirir
        public async Task<IEnumerable<WorkDto>> GetPendingApprovalWorksAsync()
        {
            return await _context.Works
                .Where(w => w.RequiresApproval && w.ApprovedByEmployeeId == null)
                .Select(w => MapToDto(w))
                .ToListAsync();
        }

        // Periyodik işleri getirir
        public async Task<IEnumerable<WorkDto>> GetRecurringWorksAsync()
        {
            return await _context.Works
                .Where(w => w.IsRecurring)
                .Select(w => MapToDto(w))
                .ToListAsync();
        }

        // Güvenlik riski olan işleri getirir
        public async Task<IEnumerable<WorkDto>> GetSafetyRiskWorksAsync()
        {
            return await _context.Works
                .Where(w => w.HasSafetyRisks)
                .Select(w => MapToDto(w))
                .ToListAsync();
        }

        // Kalite puanına göre işleri getirir
        public async Task<IEnumerable<WorkDto>> GetWorksByQualityScoreAsync(int minScore)
        {
            return await _context.Works
                .Where(w => w.QualityScore >= minScore)
                .Select(w => MapToDto(w))
                .ToListAsync();
        }

        // Yardımcı metotlar: DTO ile Model arasında dönüşüm
        private WorkDto MapToDto(Work work)
        {
            return new WorkDto
            {
                WorkId = work.WorkId,
                WorkName = work.WorkName,
                Description = work.Description,
                Status = work.Status,
                Priority = work.Priority,
                StartDate = work.StartDate,
                DueDate = work.DueDate,
                CompletionDate = work.CompletionDate,
                Location = work.Location,
                EstimatedCost = work.EstimatedCost,
                ActualCost = work.ActualCost,
                EstimatedDuration = work.EstimatedDuration,
                ActualDuration = work.ActualDuration,
                RequiredEquipment = work.RequiredEquipment,
                RequiredMaterials = work.RequiredMaterials,
                IsRecurring = work.IsRecurring,
                RecurrencePattern = work.RecurrencePattern,
                RequiresApproval = work.RequiresApproval,
                Notes = work.Notes,
                IsActive = work.IsActive,
                CancellationReason = work.CancellationReason,
                CancellationDate = work.CancellationDate,
                QualityScore = work.QualityScore,
                QualityNotes = work.QualityNotes,
                HasSafetyRisks = work.HasSafetyRisks,
                SafetyNotes = work.SafetyNotes
            };
        }

        private Work MapToModel(WorkDto workDto)
        {
            return new Work
            {
                WorkId = workDto.WorkId,
                WorkName = workDto.WorkName,
                Description = workDto.Description,
                Status = workDto.Status,
                Priority = workDto.Priority,
                StartDate = workDto.StartDate,
                DueDate = workDto.DueDate,
                CompletionDate = workDto.CompletionDate,
                Location = workDto.Location,
                EstimatedCost = workDto.EstimatedCost,
                ActualCost = workDto.ActualCost,
                EstimatedDuration = workDto.EstimatedDuration,
                ActualDuration = workDto.ActualDuration,
                RequiredEquipment = workDto.RequiredEquipment,
                RequiredMaterials = workDto.RequiredMaterials,
                IsRecurring = workDto.IsRecurring,
                RecurrencePattern = workDto.RecurrencePattern,
                RequiresApproval = workDto.RequiresApproval,
                Notes = workDto.Notes,
                IsActive = workDto.IsActive,
                CancellationReason = workDto.CancellationReason,
                CancellationDate = workDto.CancellationDate,
                QualityScore = workDto.QualityScore,
                QualityNotes = workDto.QualityNotes,
                HasSafetyRisks = workDto.HasSafetyRisks,
                SafetyNotes = workDto.SafetyNotes
            };
        }
    }
}