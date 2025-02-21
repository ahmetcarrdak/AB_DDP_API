using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDPApi.Interfaces;
using DDPApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DDPApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StationController : ControllerBase
    {
        private readonly IStation _stationService;

        public StationController(IStation stationService)
        {
            _stationService = stationService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<Station>>> GetAllStations()
        {
            var stations = await _stationService.GetAllStationsAsync();
            return Ok(stations);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Station>> GetStationById(int id)
        {
            var station = await _stationService.GetStationByIdAsync(id);
            if (station == null)
                return NotFound();
            return Ok(station);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Station>> CreateStation(Station station)
        {
            var createdStation = await _stationService.CreateStationAsync(station);
            return CreatedAtAction(nameof(GetStationById), new { id = createdStation.StationId }, createdStation);
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult<Station>> UpdateStation(int id, Station station)
        {
            var updatedStation = await _stationService.UpdateStationAsync(id, station);
            if (updatedStation == null)
                return NotFound();
            return Ok(updatedStation);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteStation(int id)
        {
            var result = await _stationService.DeleteStationAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpGet("GetActive")]
        public async Task<ActionResult<List<Station>>> GetActiveStations()
        {
            var stations = await _stationService.GetActiveStationsAsync();
            return Ok(stations);
        }

        [HttpGet("GetByType/{stationType}")]
        public async Task<ActionResult<List<Station>>> GetStationsByType(int stationType)
        {
            var stations = await _stationService.GetStationsByTypeAsync(stationType);
            return Ok(stations);
        }

        [HttpGet("GetByDepartment/{department}")]
        public async Task<ActionResult<List<Station>>> GetStationsByDepartment(string department)
        {
            var stations = await _stationService.GetStationsByDepartmentAsync(department);
            return Ok(stations);
        }

        [HttpPut("UpdateMaintenance/{id}")]
        public async Task<ActionResult> UpdateMaintenanceStatus(int id, bool maintenanceRequired)
        {
            var result = await _stationService.UpdateMaintenanceStatusAsync(id, maintenanceRequired);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpGet("GetMaintenanceRequired")]
        public async Task<ActionResult<List<Station>>> GetStationsRequiringMaintenance()
        {
            var stations = await _stationService.GetStationsRequiringMaintenanceAsync();
            return Ok(stations);
        }

        [HttpPut("UpdateQualityCheck/{id}")]
        public async Task<ActionResult> UpdateQualityCheckStatus(int id, bool requiresQualityCheck)
        {
            var result = await _stationService.UpdateQualityCheckStatusAsync(id, requiresQualityCheck);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpGet("GetByResponsiblePerson/{responsiblePersonId}")]
        public async Task<ActionResult<List<Station>>> GetStationsByResponsiblePerson(int responsiblePersonId)
        {
            var stations = await _stationService.GetStationsByResponsiblePersonAsync(responsiblePersonId);
            return Ok(stations);
        }

        [HttpGet("top-pending-stations")]
        public async Task<IActionResult> GetTopStationsWithMostPendingJobsAndStages()
        {
            try
            {
                // Servis metodunu çağırıyoruz
                var topStations = await _stationService.GetTopStationsWithMostPendingJobsAndStagesAsync();

                // Sonuç başarılı ise 200 dönüyoruz
                return Ok(topStations);
            }
            catch (System.Exception ex)
            {
                // Eğer bir hata oluşursa, hata mesajıyla birlikte 500 dönüyoruz
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
