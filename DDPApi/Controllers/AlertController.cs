using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DDPApi.Models;
using DDPApi.Interfaces;

namespace DDPApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertController : ControllerBase
    {
        private readonly IAlert _alertService;

        public AlertController(IAlert alertService)
        {
            _alertService = alertService;
        }

        // Yeni bir alarm ekler
        [HttpPost]
        public async Task<ActionResult<Alert>> AddAlertAsync([FromBody] Alert alert)
        {
            if (alert == null)
            {
                return BadRequest("Alert object cannot be null");
            }

            var addedAlert = await _alertService.AddAlertAsync(alert);
            return CreatedAtAction(nameof(GetAlertByIdAsync), new { id = addedAlert.AlertId }, addedAlert);
        }

        // Alarm günceller
        [HttpPut("{id}")]
        public async Task<ActionResult<Alert>> UpdateAlertAsync(int id, [FromBody] Alert alert)
        {
            if (alert == null)
            {
                return BadRequest("Alert object cannot be null");
            }

            var updatedAlert = await _alertService.UpdateAlertAsync(id, alert);
            if (updatedAlert == null)
            {
                return NotFound($"Alert with ID {id} not found");
            }

            return Ok(updatedAlert);
        }

        // Alarm siler
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAlertAsync(int id)
        {
            var result = await _alertService.DeleteAlertAsync(id);
            if (!result)
            {
                return NotFound($"Alert with ID {id} not found");
            }

            return NoContent(); // Successfully deleted
        }

        // ID ile alarm getirir
        [HttpGet("{id}")]
        public async Task<ActionResult<Alert>> GetAlertByIdAsync(int id)
        {
            var alert = await _alertService.GetAlertByIdAsync(id);
            if (alert == null)
            {
                return NotFound($"Alert with ID {id} not found");
            }

            return Ok(alert);
        }

        // Tüm alarmları getirir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alert>>> GetAllAlertsAsync()
        {
            var alerts = await _alertService.GetAllAlertsAsync();
            return Ok(alerts);
        }

        // Çözülmemiş alarmları getirir
        [HttpGet("unresolved")]
        public async Task<ActionResult<IEnumerable<Alert>>> GetUnresolvedAlertsAsync()
        {
            var alerts = await _alertService.GetUnresolvedAlertsAsync();
            return Ok(alerts);
        }

        // Çözülmüş alarmları getirir
        [HttpGet("resolved")]
        public async Task<ActionResult<IEnumerable<Alert>>> GetResolvedAlertsAsync()
        {
            var alerts = await _alertService.GetResolvedAlertsAsync();
            return Ok(alerts);
        }
    }
}
