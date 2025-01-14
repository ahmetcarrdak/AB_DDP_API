using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DDPApi.Models;
using DDPApi.Interfaces;

namespace DDPApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkforcePlanningController : ControllerBase
    {
        private readonly IWorkforcePlanning _workforcePlanningService;

        public WorkforcePlanningController(IWorkforcePlanning workforcePlanningService)
        {
            _workforcePlanningService = workforcePlanningService;
        }

        // Yeni bir iş gücü planlaması ekler
        [HttpPost]
        public async Task<ActionResult<WorkforcePlanning>> AddWorkforcePlanningAsync([FromBody] WorkforcePlanning workforcePlanning)
        {
            if (workforcePlanning == null)
            {
                return BadRequest("WorkforcePlanning cannot be null");
            }

            var addedPlanning = await _workforcePlanningService.AddWorkforcePlanningAsync(workforcePlanning);
            return CreatedAtAction(nameof(GetWorkforcePlanningByIdAsync), new { id = addedPlanning.WorkforcePlanningId }, addedPlanning);
        }

        // Var olan iş gücü planlamasını günceller
        [HttpPut("{id}")]
        public async Task<ActionResult<WorkforcePlanning>> UpdateWorkforcePlanningAsync(int id, [FromBody] WorkforcePlanning workforcePlanning)
        {
            if (workforcePlanning == null)
            {
                return BadRequest("WorkforcePlanning cannot be null");
            }

            var updatedPlanning = await _workforcePlanningService.UpdateWorkforcePlanningAsync(id, workforcePlanning);
            if (updatedPlanning == null)
            {
                return NotFound($"Workforce planning with ID {id} not found");
            }

            return Ok(updatedPlanning);
        }

        // İş gücü planlamasını siler
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWorkforcePlanningAsync(int id)
        {
            var result = await _workforcePlanningService.DeleteWorkforcePlanningAsync(id);
            if (!result)
            {
                return NotFound($"Workforce planning with ID {id} not found");
            }

            return NoContent();
        }

        // ID'ye göre iş gücü planlamasını getirir
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkforcePlanning>> GetWorkforcePlanningByIdAsync(int id)
        {
            var planning = await _workforcePlanningService.GetWorkforcePlanningByIdAsync(id);
            if (planning == null)
            {
                return NotFound($"Workforce planning with ID {id} not found");
            }

            return Ok(planning);
        }

        // Tüm iş gücü planlamalarını getirir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkforcePlanning>>> GetAllWorkforcePlanningsAsync()
        {
            var plannings = await _workforcePlanningService.GetAllWorkforcePlanningsAsync();
            return Ok(plannings);
        }
    }
}
