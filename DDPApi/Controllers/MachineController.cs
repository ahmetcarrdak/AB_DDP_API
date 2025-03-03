using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DDPApi.Data;
using DDPApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DDPApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MachineController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MachineController(AppDbContext context)
        {
            _context = context;
        }

        private int GetCompanyId()
        {
            return int.Parse(User.FindFirst("CompanyId")?.Value ?? "0");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Machine>>> GetMachines()
        {
            var companyId = GetCompanyId();
            return await _context.Machines
                .Where(m => m.CompanyId == companyId)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Machine>> GetMachine(int id)
        {
            var companyId = GetCompanyId();
            var machine = await _context.Machines
                .FirstOrDefaultAsync(m => m.Id == id && m.CompanyId == companyId);

            if (machine == null)
                return NotFound();

            return machine;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMachine(int id, Machine machine)
        {
            if (id != machine.Id)
                return BadRequest();

            var companyId = GetCompanyId();
            var existingMachine = await _context.Machines
                .FirstOrDefaultAsync(m => m.Id == id && m.CompanyId == companyId);

            if (existingMachine == null)
                return NotFound();

            machine.CompanyId = companyId; // Ensure company ID is preserved
            _context.Entry(existingMachine).CurrentValues.SetValues(machine);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MachineExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Machine>> CreateMachine(Machine machine)
        {
            machine.CompanyId = GetCompanyId();
            _context.Machines.Add(machine);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMachine), new { id = machine.Id }, machine);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMachine(int id)
        {
            var companyId = GetCompanyId();
            var machine = await _context.Machines
                .FirstOrDefaultAsync(m => m.Id == id && m.CompanyId == companyId);

            if (machine == null)
                return NotFound();

            _context.Machines.Remove(machine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MachineExists(int id)
        {
            var companyId = GetCompanyId();
            return _context.Machines.Any(e => e.Id == id && e.CompanyId == companyId);
        }
    }
}
