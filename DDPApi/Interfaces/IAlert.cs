using DDPApi.Models;

namespace DDPApi.Interfaces;

public interface IAlert
{
    Task<Alert> AddAlertAsync(Alert alert); // Yeni uyarı ekleme
    Task<Alert> UpdateAlertAsync(int id, Alert alert); // Uyarıyı güncelleme
    Task<bool> DeleteAlertAsync(int id); // Uyarıyı silme
    Task<Alert> GetAlertByIdAsync(int id); // Uyarıyı ID ile sorgulama
    Task<IEnumerable<Alert>> GetAllAlertsAsync(); // Tüm uyarıları listeleme
    Task<IEnumerable<Alert>> GetUnresolvedAlertsAsync(); // Çözülmemiş uyarıları listeleme
    Task<IEnumerable<Alert>> GetResolvedAlertsAsync(); // Çözülmemiş uyarıları listeleme
}