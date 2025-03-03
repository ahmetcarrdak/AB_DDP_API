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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _companyId;

        public WorkService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
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

        public async Task<IEnumerable<Work>> GetAllWorksAsync()
        {
            return await _context.Works
                .Where(w => w.CompanyId == _companyId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<WorkDto> GetWorkByIdAsync(int id)
        {
            var work = await _context.Works
                .Where(w => w.CompanyId == _companyId && w.WorkId == id)
                .FirstOrDefaultAsync();
            return work != null ? MapToDto(work) : null;
        }

        public async Task<List<WorkStationDto>> GetWorkStationAsync()
        {
            return await _context.Works
                .Where(w => w.CompanyId == _companyId)
                .Select(ws => new WorkStationDto
                {
                    StationId = ws.StationId,
                    StagesId = ws.StagesId,
                    WorkId = ws.WorkId,
                    WorkName = ws.WorkName,
                    Description = ws.Description
                })
               .ToListAsync();
        }

        public async Task<bool> AddWorkAsync(WorkDto workDto)
        {
            try
            {
                var work = new Work
                {
                    CompanyId = _companyId,
                    // Gönderilen alanları alıyoruz
                    WorkName = workDto.WorkName,
                    Priority = workDto.Priority,
                    AssignedEmployeeId = workDto.AssignedEmployeeId,
                    RequiredEquipment = workDto.RequiredEquipment,
                    RequiredMaterials = workDto.RequiredMaterials,
                    IsRecurring = workDto.IsRecurring ?? false,  // Varsayılan değer false
                    RecurrencePattern = workDto.RecurrencePattern,
                    Notes = workDto.Notes,
                    HasSafetyRisks = workDto.HasSafetyRisks ?? false,  // Varsayılan değer false
                    Description = workDto.Description ?? "Belirtilmemiş",

                    // Diğer alanları eksik bırakıyoruz ya da kendi değerlerimizi atıyoruz
                    Barcode = GenerateBarcode(),  // Otomatik olarak barcode oluşturuluyor
                    StartDate = null,  // Başlangıç tarihi boş
                    DueDate = null,  // Teslim tarihi boş
                    CompletionDate = null,  // Tamamlama tarihi boş
                    Location = null,  // Lokasyon boş
                    EstimatedCost = 0,  // Tahmini maliyet 0
                    ActualCost = 0,  // Gerçekleşen maliyet 0
                    EstimatedDuration = 0,  // Tahmini süre 0
                    ActualDuration = 0,  // Gerçekleşen süre 0
                    RequiresApproval = false,  // Onay gerektirmiyor
                    IsActive = true,  // İş aktif
                    CancellationReason = null,  // İptal nedeni boş
                    CancellationDate = null,  // İptal tarihi boş
                    QualityScore = 0,  // Kalite puanı 0
                    QualityNotes = null,  // Kalite notları boş
                    SafetyNotes = null,  // Güvenlik notları boş
                    Status = 1

                };

                await _context.Works.AddAsync(work);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddWorkAsync Error: {ex.Message}");
                return false;
            }
        }


        private string GenerateBarcode()
        {
            return $"BC-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        public async Task<bool> UpdateWorkAsync(WorkDto workDto)
        {
            var existingWork = await _context.Works
                .Where(w => w.CompanyId == _companyId && w.WorkId == workDto.WorkId)
                .FirstOrDefaultAsync();
                
            if (existingWork == null)
            {
                return false;
            }

            try
            {
                existingWork.WorkName = workDto.WorkName;
                existingWork.Description = workDto.Description;
                existingWork.Status = workDto.Status;
                existingWork.Priority = workDto.Priority;
                existingWork.StartDate = workDto.StartDate;
                existingWork.DueDate = workDto.DueDate;
                existingWork.CompletionDate = workDto.CompletionDate;
                existingWork.Location = workDto.Location;
                existingWork.EstimatedCost = workDto.EstimatedCost ?? 0;
                existingWork.ActualCost = workDto.ActualCost ?? 0;
                existingWork.EstimatedDuration = workDto.EstimatedDuration ?? 0;
                existingWork.ActualDuration = workDto.ActualDuration ?? 0;
                existingWork.RequiredEquipment = workDto.RequiredEquipment;
                existingWork.RequiredMaterials = workDto.RequiredMaterials;
                existingWork.IsRecurring = workDto.IsRecurring ?? false;
                existingWork.RecurrencePattern = workDto.RecurrencePattern;
                existingWork.RequiresApproval = workDto.RequiresApproval ?? false;
                existingWork.Notes = workDto.Notes;
                existingWork.IsActive = workDto.IsActive ?? false;
                existingWork.CancellationReason = workDto.CancellationReason;
                existingWork.CancellationDate = workDto.CancellationDate;
                existingWork.QualityScore = workDto.QualityScore ?? 0;
                existingWork.QualityNotes = workDto.QualityNotes;
                existingWork.HasSafetyRisks = workDto.HasSafetyRisks ?? false;
                existingWork.SafetyNotes = workDto.SafetyNotes;

                _context.Works.Update(existingWork);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateWorkAsync Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteWorkAsync(int id)
        {
            var work = await _context.Works
                .Where(w => w.CompanyId == _companyId && w.WorkId == id)
                .FirstOrDefaultAsync();
                
            if (work == null)
                return false;

            try
            {
                _context.Works.Remove(work);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteWorkAsync Error: {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<WorkDto>> GetWorksByEmployeeIdAsync(int employeeId)
        {
            return await _context.Works
                .Where(w => w.CompanyId == _companyId && w.AssignedEmployeeId == employeeId)
                .Select(w => MapToDto(w))
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkDto>> GetWorksByPriorityAsync(string priority)
        {
            return await _context.Works
                .Where(w => w.CompanyId == _companyId && w.Priority == priority)
                .Select(w => MapToDto(w))
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkDto>> GetWorksByStatusAsync(int status)
        {
            return await _context.Works
                .Where(w => w.CompanyId == _companyId && w.Status == status)
                .Select(w => MapToDto(w))
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkDto>> GetWorksByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Works
                .Where(w => w.CompanyId == _companyId && w.CreatedDate >= startDate && w.CreatedDate <= endDate)
                .Select(w => MapToDto(w))
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkDto>> GetDelayedWorksAsync()
        {
            return await _context.Works
                .Where(w => w.CompanyId == _companyId && w.DueDate < DateTime.Now && (w.CompletionDate == null || w.CompletionDate > w.DueDate))
                .Select(w => MapToDto(w))
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkDto>> GetCancelledWorksAsync()
        {
            return await _context.Works
                .Where(w => w.CompanyId == _companyId && w.CancellationDate != null)
                .Select(w => MapToDto(w))
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkDto>> GetRecurringWorksAsync()
        {
            return await _context.Works
                .Where(w => w.CompanyId == _companyId && w.IsRecurring == true)
                .Select(w => MapToDto(w))
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkDto>> GetPendingApprovalWorksAsync()
        {
            return await _context.Works
                .Where(w => w.CompanyId == _companyId && w.RequiresApproval == true && w.Status == 1)
                .Select(w => MapToDto(w))
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkDto>> GetSafetyRiskWorksAsync()
        {
            return await _context.Works
                .Where(w => w.CompanyId == _companyId && w.HasSafetyRisks == true)
                .Select(w => MapToDto(w))
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkDto>> GetWorksByQualityScoreAsync(int minScore)
        {
            return await _context.Works
                .Where(w => w.CompanyId == _companyId && (w.QualityScore ?? 0) >= minScore)
                .Select(w => MapToDto(w))
                .ToListAsync();
        }

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
                EstimatedCost = work.EstimatedCost ?? 0,
                ActualCost = work.ActualCost ?? 0,
                EstimatedDuration = work.EstimatedDuration ?? 0,
                ActualDuration = work.ActualDuration ?? 0,
                RequiredEquipment = work.RequiredEquipment,
                RequiredMaterials = work.RequiredMaterials,
                IsRecurring = work.IsRecurring ?? false,
                RecurrencePattern = work.RecurrencePattern,
                RequiresApproval = work.RequiresApproval ?? false,
                Notes = work.Notes,
                IsActive = work.IsActive ?? false,
                CancellationReason = work.CancellationReason,
                CancellationDate = work.CancellationDate,
                QualityScore = work.QualityScore ?? 0,
                QualityNotes = work.QualityNotes,
                HasSafetyRisks = work.HasSafetyRisks ?? false,
                SafetyNotes = work.SafetyNotes,
                Barcode = work.Barcode
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
                EstimatedCost = workDto.EstimatedCost ?? 0,
                ActualCost = workDto.ActualCost ?? 0,
                EstimatedDuration = workDto.EstimatedDuration ?? 0,
                ActualDuration = workDto.ActualDuration ?? 0,
                RequiredEquipment = workDto.RequiredEquipment,
                RequiredMaterials = workDto.RequiredMaterials,
                IsRecurring = workDto.IsRecurring ?? false,
                RecurrencePattern = workDto.RecurrencePattern,
                RequiresApproval = workDto.RequiresApproval ?? false,
                Notes = workDto.Notes,
                IsActive = workDto.IsActive ?? false,
                CancellationReason = workDto.CancellationReason,
                CancellationDate = workDto.CancellationDate,
                QualityScore = workDto.QualityScore ?? 0,
                QualityNotes = workDto.QualityNotes,
                HasSafetyRisks = workDto.HasSafetyRisks ?? false,
                SafetyNotes = workDto.SafetyNotes,
                Barcode = workDto.Barcode
            };
        }
    }
}
