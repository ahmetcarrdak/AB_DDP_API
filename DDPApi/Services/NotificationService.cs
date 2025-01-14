using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DDPApi.Models; 
using DDPApi.Data; 
using DDPApi.Interfaces;

namespace DDPApi.Services
{
    public class NotificationService : INotification
    {
        private readonly AppDbContext _context;

        public NotificationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Notification> AddNotificationAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<Notification> UpdateNotificationAsync(int id, Notification notification)
        {
            var existingNotification = await _context.Notifications.FindAsync(id);
            if (existingNotification != null)
            {
                existingNotification.Message = notification.Message;
                existingNotification.NotificationDate = notification.NotificationDate;
                existingNotification.IsRead = notification.IsRead;
                existingNotification.UserId = notification.UserId;
                
                await _context.SaveChangesAsync();
            }
            return existingNotification;
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Notification> GetNotificationByIdAsync(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync()
        {
            return await _context.Notifications.Where(n => !n.IsRead).ToListAsync();
        }
    }
}
