using DDPApi.Models;

namespace DDPApi.Interfaces;

public interface INotification
{
    Task<Notification> AddNotificationAsync(Notification notification); // Yeni bildirim ekleme
    Task<Notification> UpdateNotificationAsync(int id, Notification notification); // Bildirimi güncelleme
    Task<bool> DeleteNotificationAsync(int id); // Bildirimi silme
    Task<Notification> GetNotificationByIdAsync(int id); // Bildirimi ID ile sorgulama
    Task<IEnumerable<Notification>> GetAllNotificationsAsync(); // Tüm bildirimleri listeleme
    Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(); // Okunmamış bildirimleri listeleme
}