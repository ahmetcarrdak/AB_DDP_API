using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DDPApi.Models;
using DDPApi.Interfaces;

namespace DDPApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceRecordController : ControllerBase
    {
        private readonly IMaintenanceRecord _maintenanceRecordService;

        public MaintenanceRecordController(IMaintenanceRecord maintenanceRecordService)
        {
            _maintenanceRecordService = maintenanceRecordService;
        }

        // Yeni bir bakım kaydı ekler
        [HttpPost]
        public async Task<ActionResult<MaintenanceRecord>> AddMaintenanceRecordAsync([FromBody] MaintenanceRecord maintenanceRecord)
        {
            if (maintenanceRecord == null)
            {
                return BadRequest("Maintenance record cannot be null");
            }

            var addedRecord = await _maintenanceRecordService.AddMaintenanceRecordAsync(maintenanceRecord);
            return CreatedAtAction(nameof(GetMaintenanceRecordByIdAsync), new { id = addedRecord.MaintenanceRecordId }, addedRecord);
        }

        // Var olan bakım kaydını günceller
        [HttpPut("{id}")]
        public async Task<ActionResult<MaintenanceRecord>> UpdateMaintenanceRecordAsync(int id, [FromBody] MaintenanceRecord maintenanceRecord)
        {
            if (maintenanceRecord == null)
            {
                return BadRequest("Maintenance record cannot be null");
            }

            var updatedRecord = await _maintenanceRecordService.UpdateMaintenanceRecordAsync(id, maintenanceRecord);
            if (updatedRecord == null)
            {
                return NotFound($"Maintenance record with ID {id} not found");
            }

            return Ok(updatedRecord);
        }

        // Bakım kaydını siler
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMaintenanceRecordAsync(int id)
        {
            var result = await _maintenanceRecordService.DeleteMaintenanceRecordAsync(id);
            if (!result)
            {
                return NotFound($"Maintenance record with ID {id} not found");
            }

            return NoContent();
        }

        // ID ile bakım kaydını getirir
        [HttpGet("{id}")]
        public async Task<ActionResult<MaintenanceRecord>> GetMaintenanceRecordByIdAsync(int id)
        {
            var record = await _maintenanceRecordService.GetMaintenanceRecordByIdAsync(id);
            if (record == null)
            {
                return NotFound($"Maintenance record with ID {id} not found");
            }

            return Ok(record);
        }

        // Tüm bakım kayıtlarını getirir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaintenanceRecord>>> GetAllMaintenanceRecordsAsync()
        {
            var records = await _maintenanceRecordService.GetAllMaintenanceRecordsAsync();
            return Ok(records);
        }
    }
}
