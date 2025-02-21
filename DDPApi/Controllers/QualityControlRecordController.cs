using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DDPApi.Models;
using DDPApi.Interfaces;

namespace DDPApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QualityControlRecordController : ControllerBase
    {
        private readonly IQualityControlRecord _qualityControlRecordService;

        public QualityControlRecordController(IQualityControlRecord qualityControlRecordService)
        {
            _qualityControlRecordService = qualityControlRecordService;
        }

        // Yeni bir kalite kontrol kaydı ekler
        [HttpPost("Create")]
        public async Task<ActionResult<QualityControlRecord>> AddQualityControlRecordAsync([FromBody] QualityControlRecord qualityControlRecord)
        {
            if (qualityControlRecord == null)
            {
                return BadRequest("Quality Control Record cannot be null");
            }

            var addedRecord = await _qualityControlRecordService.AddQualityControlRecordAsync(qualityControlRecord);
            return Ok(addedRecord);
        }

        // Var olan kalite kontrol kaydını günceller
        [HttpPut("{id}")]
        public async Task<ActionResult<QualityControlRecord>> UpdateQualityControlRecordAsync(int id, [FromBody] QualityControlRecord qualityControlRecord)
        {
            if (qualityControlRecord == null)
            {
                return BadRequest("Quality Control Record cannot be null");
            }

            var updatedRecord = await _qualityControlRecordService.UpdateQualityControlRecordAsync(id, qualityControlRecord);
            if (updatedRecord == null)
            {
                return NotFound($"Quality Control Record with ID {id} not found");
            }

            return Ok(updatedRecord);
        }

        // Kalite kontrol kaydını siler
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQualityControlRecordAsync(int id)
        {
            var result = await _qualityControlRecordService.DeleteQualityControlRecordAsync(id);
            if (!result)
            {
                return NotFound($"Quality Control Record with ID {id} not found");
            }

            return NoContent();
        }

        // ID ile kalite kontrol kaydını getirir
        [HttpGet("{id}")]
        public async Task<ActionResult<QualityControlRecord>> GetQualityControlRecordByIdAsync(int id)
        {
            var record = await _qualityControlRecordService.GetQualityControlRecordByIdAsync(id);
            if (record == null)
            {
                return NotFound($"Quality Control Record with ID {id} not found");
            }

            return Ok(record);
        }

        // Tüm kalite kontrol kayıtlarını getirir
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<QualityControlRecord>>> GetAllQualityControlRecordsAsync()
        {
            var records = await _qualityControlRecordService.GetAllQualityControlRecordsAsync();
            return Ok(records);
        }
    }
}
