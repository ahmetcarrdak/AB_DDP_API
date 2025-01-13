using DDPApi.Interfaces;
using DDPApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DDPApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkController : ControllerBase
    {
        private readonly IWork _workService;

        public WorkController(IWork workService)
        {
            _workService = workService;
        }

        // GET: api/Work/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Work>>> GetAllWorks()
        {
            var works = await _workService.GetAllWorksAsync();
            return Ok(works);
        }

        // GET: api/Work/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Work>>> GetActiveWorks()
        {
            var works = await _workService.GetActiveWorksAsync();
            return Ok(works);
        }

        // GET: api/Work/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Work>> GetWork(int id)
        {
            var work = await _workService.GetWorkByIdAsync(id);
            if (work == null)
                return NotFound();
            return Ok(work);
        }

        // POST: api/Work
        [HttpPost]
        public async Task<ActionResult<Work>> CreateWork(Work work)
        {
            var result = await _workService.AddWorkAsync(work);
            if (!result)
                return BadRequest();
            return CreatedAtAction(nameof(GetWork), new { id = work.WorkId }, work);
        }

        // PUT: api/Work/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWork(int id, Work work)
        {
            if (id != work.WorkId)
                return BadRequest();

            var result = await _workService.UpdateWorkAsync(work);
            if (!result)
                return NotFound();
            return NoContent();
        }

        // DELETE: api/Work/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWork(int id)
        {
            var result = await _workService.DeleteWorkAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        // GET: api/Work/department/{departmentId}
        [HttpGet("department/{departmentId}")]
        public async Task<ActionResult<IEnumerable<Work>>> GetWorksByDepartment(int departmentId)
        {
            var works = await _workService.GetWorksByDepartmentIdAsync(departmentId);
            return Ok(works);
        }

        // GET: api/Work/employee/{employeeId}
        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<Work>>> GetWorksByEmployee(int employeeId)
        {
            var works = await _workService.GetWorksByEmployeeIdAsync(employeeId);
            return Ok(works);
        }

        // GET: api/Work/priority/{priority}
        [HttpGet("priority/{priority}")]
        public async Task<ActionResult<IEnumerable<Work>>> GetWorksByPriority(string priority)
        {
            var works = await _workService.GetWorksByPriorityAsync(priority);
            return Ok(works);
        }

        // GET: api/Work/status/{status}
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Work>>> GetWorksByStatus(string status)
        {
            var works = await _workService.GetWorksByStatusAsync(status);
            return Ok(works);
        }

        // GET: api/Work/daterange
        [HttpGet("daterange")]
        public async Task<ActionResult<IEnumerable<Work>>> GetWorksByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var works = await _workService.GetWorksByDateRangeAsync(startDate, endDate);
            return Ok(works);
        }

        // GET: api/Work/delayed
        [HttpGet("delayed")]
        public async Task<ActionResult<IEnumerable<Work>>> GetDelayedWorks()
        {
            var works = await _workService.GetDelayedWorksAsync();
            return Ok(works);
        }

        // GET: api/Work/cancelled
        [HttpGet("cancelled")]
        public async Task<ActionResult<IEnumerable<Work>>> GetCancelledWorks()
        {
            var works = await _workService.GetCancelledWorksAsync();
            return Ok(works);
        }

        // GET: api/Work/recurring
        [HttpGet("recurring")]
        public async Task<ActionResult<IEnumerable<Work>>> GetRecurringWorks()
        {
            var works = await _workService.GetRecurringWorksAsync();
            return Ok(works);
        }

        // GET: api/Work/pending-approval
        [HttpGet("pending-approval")]
        public async Task<ActionResult<IEnumerable<Work>>> GetPendingApprovalWorks()
        {
            var works = await _workService.GetPendingApprovalWorksAsync();
            return Ok(works);
        }

        // GET: api/Work/safety-risk
        [HttpGet("safety-risk")]
        public async Task<ActionResult<IEnumerable<Work>>> GetSafetyRiskWorks()
        {
            var works = await _workService.GetSafetyRiskWorksAsync();
            return Ok(works);
        }

        // GET: api/Work/quality-score/{minScore}
        [HttpGet("quality-score/{minScore}")]
        public async Task<ActionResult<IEnumerable<Work>>> GetWorksByQualityScore(int minScore)
        {
            var works = await _workService.GetWorksByQualityScoreAsync(minScore);
            return Ok(works);
        }
    }
}
