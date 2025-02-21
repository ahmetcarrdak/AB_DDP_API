using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDPApi.Models;
using DDPApi.DTO;

namespace DDPApi.Interfaces
{
    public interface IWork
    {
        // Tüm işleri getirir
        Task<IEnumerable<Work>> GetAllWorksAsync();

        // Aktif işleri getirir
        Task<IEnumerable<WorkDto>> GetActiveWorksAsync();

        // ID'ye göre iş getirir
        Task<WorkDto> GetWorkByIdAsync(int id);
        
        // Belirli bir ürünün istasyon bilgilerini getirir
        Task<List<WorkStationDto>> GetWorkStationAsync();

        // Yeni iş ekler
        Task<bool> AddWorkAsync(WorkDto workDto);

        // İş bilgilerini günceller
        Task<bool> UpdateWorkAsync(WorkDto workDto);

        // İş siler
        Task<bool> DeleteWorkAsync(int id);

        // Belirli bir personele atanmış işleri getirir
        Task<IEnumerable<WorkDto>> GetWorksByEmployeeIdAsync(int employeeId);

        // Öncelik durumuna göre işleri getirir
        Task<IEnumerable<WorkDto>> GetWorksByPriorityAsync(string priority);

        // İş durumuna göre işleri getirir
        Task<IEnumerable<WorkDto>> GetWorksByStatusAsync(int status);

        // Belirli bir tarih aralığındaki işleri getirir
        Task<IEnumerable<WorkDto>> GetWorksByDateRangeAsync(DateTime startDate, DateTime endDate);

        // Gecikmiş işleri getirir
        Task<IEnumerable<WorkDto>> GetDelayedWorksAsync();

        // İptal edilmiş işleri getirir
        Task<IEnumerable<WorkDto>> GetCancelledWorksAsync();

        // Periyodik işleri getirir
        Task<IEnumerable<WorkDto>> GetRecurringWorksAsync();

        // Onay bekleyen işleri getirir
        Task<IEnumerable<WorkDto>> GetPendingApprovalWorksAsync();

        // Güvenlik riski olan işleri getirir
        Task<IEnumerable<WorkDto>> GetSafetyRiskWorksAsync();

        // Kalite puanına göre işleri getirir
        Task<IEnumerable<WorkDto>> GetWorksByQualityScoreAsync(int minScore);
    }
}