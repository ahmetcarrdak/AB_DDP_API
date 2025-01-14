namespace DDPApi.Models;

public class Notification
{
    public int NotificationId { get; set; }           // Bildirim ID'si
    public int UserId { get; set; }                   // Bildirimi alan kullanıcı ID'si
    public string Message { get; set; }               // Bildirim mesajı
    public DateTime NotificationDate { get; set; }    // Bildirim tarihi
    public bool IsRead { get; set; }                  // Okunup okunmadığı
    public NotificationType Type { get; set; }        // Bildirim türü (Arıza, Bakım, vb.)
}

public enum NotificationType
{
    Fault,
    Maintenance,
    Inventory,
    QualityControl,
    General
}
