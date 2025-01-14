using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DDPApi.Interfaces;
using DDPApi.Models;

namespace DDPApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineController : ControllerBase
    {
        private readonly IMachine _machineService;

        // Constructor ile IMachineService bağımlılığı alınır
        public MachineController(IMachine machineService)
        {
            _machineService = machineService;
        }

        // Tüm makineleri getirir
        [HttpGet]
        public async Task<IActionResult> GetAllMachines()
        {
            var machines = await _machineService.GetAllMachinesAsync();
            return Ok(machines); // 200 OK ve veri döndürür
        }

        // ID'ye göre bir makine getirir
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMachineById(int id)
        {
            var machine = await _machineService.GetMachineByIdAsync(id);
            if (machine == null)
                return NotFound(); // 404 Not Found
            return Ok(machine); // 200 OK
        }

        // Yeni bir makine ekler
        [HttpPost]
        public async Task<IActionResult> AddMachine([FromBody] Machine machine)
        {
            if (machine == null)
                return BadRequest("Makine bilgisi geçersiz."); // 400 Bad Request

            var createdMachine = await _machineService.AddMachineAsync(machine);
            return CreatedAtAction(nameof(GetMachineById), new { id = createdMachine.MachineId }, createdMachine); // 201 Created
        }

        // ID'ye göre bir makineyi günceller
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMachine(int id, [FromBody] Machine updatedMachine)
        {
            if (updatedMachine == null)
                return BadRequest("Makine bilgisi geçersiz."); // 400 Bad Request

            var machine = await _machineService.UpdateMachineAsync(id, updatedMachine);
            if (machine == null)
                return NotFound(); // 404 Not Found

            return Ok(machine); // 200 OK
        }

        // ID'ye göre bir makineyi siler
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMachine(int id)
        {
            var isDeleted = await _machineService.DeleteMachineAsync(id);
            if (!isDeleted)
                return NotFound(); // 404 Not Found

            return NoContent(); // 204 No Content
        }

        // Çalışır durumda olan makineleri getirir
        [HttpGet("operational")]
        public async Task<IActionResult> GetOperationalMachines()
        {
            var machines = await _machineService.GetOperationalMachinesAsync();
            return Ok(machines); // 200 OK
        }

        // Çalışmayan makineleri getirir
        [HttpGet("non-operational")]
        public async Task<IActionResult> GetNonOperationalMachines()
        {
            var machines = await _machineService.GetNonOperationalMachinesAsync();
            return Ok(machines); // 200 OK
        }

        // Belirtilen lokasyondaki makineleri getirir
        [HttpGet("location/{location}")]
        public async Task<IActionResult> GetMachinesByLocation(string location)
        {
            var machines = await _machineService.GetMachinesByLocationAsync(location);
            return Ok(machines); // 200 OK
        }
    }
}
