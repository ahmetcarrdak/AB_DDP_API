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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMachine(int id)
        {
            var result = await _machineService.DeleteMachineAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
