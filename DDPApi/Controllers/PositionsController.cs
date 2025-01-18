using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DDPApi.Interfaces;
using DDPApi.Models;

namespace DDPApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PositionsController : ControllerBase
    {
        private readonly IPositions _positionsService;

        public PositionsController(IPositions positionsService)
        {
            _positionsService = positionsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Positions>>> GetPositions()
        {
            var positions = await _positionsService.GetAllPositions();
            return Ok(positions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Positions>> GetPosition(int id)
        {
            var position = await _positionsService.GetPositionById(id);
            if (position == null)
            {
                return NotFound();
            }
            return Ok(position);
        }

        [HttpPost]
        public async Task<ActionResult<Positions>> CreatePosition(Positions position)
        {
            var createdPosition = await _positionsService.CreatePosition(position);
            return CreatedAtAction(nameof(GetPosition), new { id = createdPosition.PositionId }, createdPosition);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePosition(int id, Positions position)
        {
            if (id != position.PositionId)
            {
                return BadRequest();
            }

            var updatedPosition = await _positionsService.UpdatePosition(position);
            return Ok(updatedPosition);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            var result = await _positionsService.DeletePosition(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}