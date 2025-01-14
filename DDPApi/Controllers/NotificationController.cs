using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DDPApi.Models;
using DDPApi.Interfaces;

namespace DDPApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotification _notificationService;

        public NotificationController(INotification notificationService)
        {
            _notificationService = notificationService;
        }

        // Yeni bir bildirim ekler
        [HttpPost]
        public async Task<ActionResult<Notification>> AddNotificationAsync([FromBody] Notification notification)
        {
            if (notification == null)
            {
                return BadRequest("Notification cannot be null");
            }

            var addedNotification = await _notificationService.AddNotificationAsync(notification);
            return CreatedAtAction(nameof(GetNotificationByIdAsync), new { id = addedNotification.NotificationId }, addedNotification);
        }

        // Var olan bildirimi günceller
        [HttpPut("{id}")]
        public async Task<ActionResult<Notification>> UpdateNotificationAsync(int id, [FromBody] Notification notification)
        {
            if (notification == null)
            {
                return BadRequest("Notification cannot be null");
            }

            var updatedNotification = await _notificationService.UpdateNotificationAsync(id, notification);
            if (updatedNotification == null)
            {
                return NotFound($"Notification with ID {id} not found");
            }

            return Ok(updatedNotification);
        }

        // Bildirimi siler
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNotificationAsync(int id)
        {
            var result = await _notificationService.DeleteNotificationAsync(id);
            if (!result)
            {
                return NotFound($"Notification with ID {id} not found");
            }

            return NoContent();
        }

        // ID ile bildirimi getirir
        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetNotificationByIdAsync(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                return NotFound($"Notification with ID {id} not found");
            }

            return Ok(notification);
        }

        // Tüm bildirimleri getirir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetAllNotificationsAsync()
        {
            var notifications = await _notificationService.GetAllNotificationsAsync();
            return Ok(notifications);
        }

        // Okunmamış bildirimleri getirir
        [HttpGet("unread")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetUnreadNotificationsAsync()
        {
            var unreadNotifications = await _notificationService.GetUnreadNotificationsAsync();
            return Ok(unreadNotifications);
        }
    }
}
