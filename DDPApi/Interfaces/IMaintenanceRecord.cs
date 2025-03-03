using DDPApi.Models;

namespace DDPApi.Interfaces;

public interface IMaintenanceRecord
{
    Task<MaintenanceRecord> AddMaintenanceRecordAsync(MaintenanceRecord maintenanceRecord); // Yeni bakım kaydı ekleme
    Task<MaintenanceRecord> UpdateMaintenanceRecordAsync(int id, MaintenanceRecord maintenanceRecord); // Bakım kaydını güncelleme
    Task<bool> DeleteMaintenanceRecordAsync(int id); // Bakım kaydını silme
    Task<MaintenanceRecord> GetMaintenanceRecordByIdAsync(int id); // Bakım kaydını ID ile sorgulama
    Task<IEnumerable<MaintenanceRecord>> GetAllMaintenanceRecordsAsync(); // Tüm bakım kayıtlarını listeleme
}