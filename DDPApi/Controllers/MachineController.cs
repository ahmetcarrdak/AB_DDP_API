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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMachine(int id, Machine machine)
        {
            if (id != machine.Id)
                return BadRequest();

            var updatedMachine = await _machineService.UpdateMachineAsync(id, machine);

            if (updatedMachine == null)
                return NotFound();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Machine>> CreateMachine(Machine machine)
        {
            var createdMachine = await _machineService.AddMachineAsync(machine);
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
