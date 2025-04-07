using DDPApi.DTO;
using DDPApi.Interfaces;
using DDPApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDPApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MachineController : ControllerBase
    {
        private readonly IMachine _machineService;

        public MachineController(IMachine machineService)
        {
            _machineService = machineService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Machine>>> GetMachines()
        {
            var machines = await _machineService.GetAllMachinesAsync();
            return Ok(machines);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Machine>> GetMachine(int id)
        {
            var machine = await _machineService.GetMachineByIdAsync(id);

            if (machine == null)
                return NotFound();

            return Ok(machine);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMachine(MachineUpdateDto updatedMachineDto)
        {
            var updatedMachine = await _machineService.UpdateMachineAsync(updatedMachineDto);

            if (updatedMachine == null)
                return NotFound();

            return Ok("Updated");
        }

        [HttpPost]
        public async Task<ActionResult<Machine>> CreateMachine(MachineCreateDto machineDto)
        {
            var createdMachine = await _machineService.AddMachineAsync(machineDto);
            return CreatedAtAction(nameof(GetMachine), new { id = createdMachine.Id }, createdMachine);
        }

        [HttpPut("Deleted/{id}")]
        public async Task<IActionResult> DeleteMachine(int id)
        {
            var result = await _machineService.DeleteMachineAsync(id);

            if (!result)
                return NotFound();

            return Ok("success");
        }
        
        
        [HttpPut("{id}/Restore")]
        public async Task<IActionResult> RestoreMachine(int id)
        {
            var result = await _machineService.RestoreMachineAsync(id);

            if (!result)
                return NotFound();

            return Ok("success");
        }
        
        // Silinmiş makineler
        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeleted()
        {
            var deletedMachines = await _machineService.GetDeletedMachinesAsync();
            return Ok(deletedMachines);
        }
    }
}
