using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDPApi.Models;

namespace DDPApi.Interfaces
{
    public interface IWork
    {
        // Tüm işleri getirir
        Task<IEnumerable<Work>> GetAllWorksAsync();

        // Aktif işleri getirir
        Task<IEnumerable<Work>> GetActiveWorksAsync();

        // ID'ye göre iş getirir
        Task<Work> GetWorkByIdAsync(int id);

        // Yeni iş ekler
        Task<bool> AddWorkAsync(Work work);

        // İş bilgilerini günceller
        Task<bool> UpdateWorkAsync(Work work);

        // İş siler
        Task<bool> DeleteWorkAsync(int id);

        // Belirli bir departmana ait işleri getirir
        Task<IEnumerable<Work>> GetWorksByDepartmentIdAsync(int departmentId);

        // Belirli bir personele atanmış işleri getirir
        Task<IEnumerable<Work>> GetWorksByEmployeeIdAsync(int employeeId);

        // Öncelik durumuna göre işleri getirir
        Task<IEnumerable<Work>> GetWorksByPriorityAsync(string priority);

        // İş durumuna göre işleri getirir
        Task<IEnumerable<Work>> GetWorksByStatusAsync(string status);

        // Belirli bir tarih aralığındaki işleri getirir
        Task<IEnumerable<Work>> GetWorksByDateRangeAsync(DateTime startDate, DateTime endDate);

        // Gecikmiş işleri getirir
        Task<IEnumerable<Work>> GetDelayedWorksAsync();

        // İptal edilmiş işleri getirir
        Task<IEnumerable<Work>> GetCancelledWorksAsync();

        // Periyodik işleri getirir
        Task<IEnumerable<Work>> GetRecurringWorksAsync();

        // Onay bekleyen işleri getirir
        Task<IEnumerable<Work>> GetPendingApprovalWorksAsync();

        // Güvenlik riski olan işleri getirir
        Task<IEnumerable<Work>> GetSafetyRiskWorksAsync();

        // Kalite puanına göre işleri getirir
        Task<IEnumerable<Work>> GetWorksByQualityScoreAsync(int minScore);
    }
}

