using Microsoft.AspNetCore.Mvc;
using DDPApi.Interfaces;
using DDPApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace DDPApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class MachineFaultController : ControllerBase
{
    private readonly IMachineFault _machineFaultService;

    public MachineFaultController(IMachineFault machineFaultService)
    {
        _machineFaultService = machineFaultService;
    }

    // GET: api/machinefault
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MachineFault>>> GetAllFaults()
    {
        var faults = await _machineFaultService.GetAllFaultsAsync();
        return Ok(faults);
    }

    // GET: api/machinefault/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MachineFault>> GetFaultById(int id)
    {
        var fault = await _machineFaultService.GetFaultByIdAsync(id);
        if (fault == null)
        {
            return NotFound(new { Message = $"Fault with ID {id} not found." });
        }

        return Ok(fault);
    }

    // GET: api/machinefault/unresolved
    [HttpGet("unresolved")]
    public async Task<ActionResult<IEnumerable<MachineFault>>> GetUnresolvedFaults()
    {
        var faults = await _machineFaultService.GetUnresolvedFaultsAsync();
        return Ok(faults);
    }

    // GET: api/machinefault/resolved
    [HttpGet("resolved")]
    public async Task<ActionResult<IEnumerable<MachineFault>>> GetResolvedFaults()
    {
        var faults = await _machineFaultService.GetResolvedFaultsAsync();
        return Ok(faults);
    }

    // GET: api/machinefault/machine/{machineCode}
    [HttpGet("machine/{machineCode}")]
    public async Task<ActionResult<IEnumerable<MachineFault>>> GetFaultsByMachineCode(string machineCode)
    {
        var faults = await _machineFaultService.GetFaultsByMachineCodeAsync(machineCode);
        return Ok(faults);
    }

    // POST: api/machinefault
    [HttpPost]
    public async Task<ActionResult<MachineFault>> AddFault([FromBody] MachineFault fault)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdFault = await _machineFaultService.AddFaultAsync(fault);
        return CreatedAtAction(nameof(GetFaultById), new { id = createdFault.FaultId }, createdFault);
    }

    // PUT: api/machinefault/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<MachineFault>> UpdateFault(int id, [FromBody] MachineFault fault)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingFault = await _machineFaultService.GetFaultByIdAsync(id);
        if (existingFault == null)
        {
            return NotFound(new { Message = $"Fault with ID {id} not found." });
        }

        var updatedFault = await _machineFaultService.UpdateFaultAsync(id, fault);
        return Ok(updatedFault);
    }

    // DELETE: api/machinefault/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteFault(int id)
    {
        var fault = await _machineFaultService.GetFaultByIdAsync(id);
        if (fault == null)
        {
            return NotFound(new { Message = $"Fault with ID {id} not found." });
        }

        var isDeleted = await _machineFaultService.DeleteFaultAsync(id);
        if (isDeleted)
        {
            return NoContent();
        }

        return StatusCode(500, new { Message = "An error occurred while deleting the fault." });
    }

    // Toplam arıza sayısını getirir
    [HttpGet("totalFault")]
    public async Task<ActionResult<int>> GetTotalFaults()
    {
        var totalFaults = await _machineFaultService.GetAllFaultsAsync();
        return Ok(totalFaults);
    }

    // En çok arıza yapan 5 makina
    [HttpGet("TotalFault5")]
    public async Task<ActionResult<Machine>> GetTotal5Fault()
    {
        var total5Faults = await _machineFaultService.GetTop5MachinesWithMostFaultsAsync();
        return Ok(total5Faults);
    }

    // En son 5 arıza kaydı yapılan makineleri getirir
    [HttpGet("latest-faults")]
    public async Task<IActionResult> GetLatest5FaultMachines()
    {
        var machines = await _machineFaultService.GetLatest5FaultMachinesAsync();
        
        if (machines == null || !machines.Any())
        {
            return NotFound("Son arızalara sahip makineler bulunamadı.");
        }

        return Ok(machines);
    }
}