using DDPApi.Interfaces;
using DDPApi.Models;
using DDPApi.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDPApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class PersonController : ControllerBase
    {
        private readonly IPerson _personService;
        private readonly ILogger<PersonService> _logger;

        public PersonController(IPerson personService, ILogger<PersonService> logger)
        {
            _personService = personService;
            _logger = logger;
        }

        // GET: api/Person/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<PersonDto>>> GetAllPersons()
        {
            var persons = await _personService.GetAllPersonsAsync();
            return Ok(persons);
        }

        // GET: api/Person/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDto>> GetPerson(int id)
        {
            var person = await _personService.GetPersonByIdAsync(id);
            if (person == null)
                return NotFound();
            return Ok(person);
        }

        // POST: api/Person
        [HttpPost("create")]
        public async Task<ActionResult<PersonDto>> CreatePerson(PersonDto personDto)
        {
            var result = await _personService.AddPersonAsync(personDto);
            if (result == null)
                return BadRequest();
            return Ok(result);
        }

        // PUT: api/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdatePerson([FromBody] PersonUpdateDto personUpdateDto)
        {
            if (personUpdateDto.Id == 0)
            {
                return BadRequest("ID is required in the request body.");
            }

            try
            {
                // Güncellenmiş kişiyi al
                var updatedPerson = await _personService.UpdatePersonAsync(personUpdateDto);

                // Başarılı bir güncelleme sonrası 200 OK ile geri döner
                return Ok(updatedPerson);
            }
            catch (ArgumentException)
            {
                // Kişi bulunamadıysa 404 döner
                return NotFound("Person not found.");
            }
        }

        // DELETE: api/Person/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var result = await _personService.DeletePersonAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        // POST: api/Person/collective-update
        [HttpPost("collective-update")]
        public async Task<IActionResult> CollectivePersonUpdate(
            [FromBody] List<PersonCollectiveUpdateDto> personUpdates)
        {
            if (personUpdates == null || personUpdates.Count == 0)
            {
                return BadRequest("Güncellenecek personel verileri bulunamadı.");
            }

            var result = await _personService.CollectivePersonUpdateAsync(personUpdates);

            if (!result)
            {
                return NotFound("Güncelleme işlemi başarısız oldu.");
            }

            return Ok("Personel verileri başarıyla güncellendi.");
        }
    }
}