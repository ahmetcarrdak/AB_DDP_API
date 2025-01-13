
using DDPApi.Interfaces;
using DDPApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DDPApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPerson _personService;

        public PersonController(IPerson personService)
        {
            _personService = personService;
        }

        // GET: api/Person/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Person>>> GetAllPersons()
        {
            var persons = await _personService.GetAllPersonsAsync();
            return Ok(persons);
        }

        // GET: api/Person/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Person>>> GetActivePersons()
        {
            var persons = await _personService.GetActivePersonsAsync();
            return Ok(persons);
        }

        // GET: api/Person/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await _personService.GetPersonByIdAsync(id);
            if (person == null)
                return NotFound();
            return Ok(person);
        }

        // POST: api/Person
        [HttpPost]
        public async Task<ActionResult<Person>> CreatePerson(Person person)
        {
            var result = await _personService.AddPersonAsync(person);
            if (result == null)
                return BadRequest();
            return CreatedAtAction(nameof(GetPerson), new { id = result.Id }, result);
        }

        // PUT: api/Person/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, Person person)
        {
            if (id != person.Id)
                return BadRequest();

            var result = await _personService.UpdatePersonAsync(person);
            if (result == null)
                return NotFound();
            return NoContent();
        }

        // DELETE: api/Person/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var result = await _personService.DeletePersonAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        // GET: api/Person/department/{department}
        [HttpGet("department/{department}")]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersonsByDepartment(string department)
        {
            var persons = await _personService.GetPersonsByDepartmentAsync(department);
            return Ok(persons);
        }

        // GET: api/Person/identity/{identityNumber}
        [HttpGet("identity/{identityNumber}")]
        public async Task<ActionResult<Person>> GetPersonByIdentityNumber(string identityNumber)
        {
            var person = await _personService.GetPersonByIdentityNumberAsync(identityNumber);
            if (person == null)
                return NotFound();
            return Ok(person);
        }

        // GET: api/Person/healthcheck
        [HttpGet("healthcheck")]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersonsWithExpiredHealthCheck()
        {
            var persons = await _personService.GetPersonsWithExpiredHealthCheckAsync();
            return Ok(persons);
        }

        // GET: api/Person/{id}/vacation
        [HttpGet("{id}/vacation")]
        public async Task<ActionResult<int>> GetRemainingVacationDays(int id)
        {
            var days = await _personService.GetRemainingVacationDaysAsync(id);
            return Ok(days);
        }

        // GET: api/Person/shift/{schedule}
        [HttpGet("shift/{schedule}")]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersonsByShiftSchedule(string schedule)
        {
            var persons = await _personService.GetPersonsByShiftScheduleAsync(schedule);
            return Ok(persons);
        }

        // PUT: api/Person/{id}/emergency
        [HttpPut("{id}/emergency")]
        public async Task<IActionResult> UpdateEmergencyContact(int id, [FromBody] EmergencyContactUpdate contact)
        {
            var result = await _personService.UpdateEmergencyContactAsync(id, contact.Contact, contact.Phone);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }

    public class EmergencyContactUpdate
    {
        public string Contact { get; set; }
        public string Phone { get; set; }
    }
}


