using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDPApi.Interfaces;
using DDPApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDPApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class StagesController : ControllerBase
    {
        private readonly IStages _stagesService;

        public StagesController(IStages stagesService)
        {
            _stagesService = stagesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stages>>> GetAllStages()
        {
            var stages = await _stagesService.GetAllStages();
            return Ok(stages);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Stages>> GetStageById(int id)
        {
            var stage = await _stagesService.GetStageById(id);
            if (stage == null)
                return NotFound();
            return Ok(stage);
        }

        [HttpPost]
        public async Task<ActionResult<Stages>> CreateStage(Stages stage)
        {
            var createdStage = await _stagesService.CreateStage(stage);
            return CreatedAtAction(nameof(GetStageById), new { id = createdStage.StageId }, createdStage);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Stages>> UpdateStage(int id, Stages stage)
        {
            if (id != stage.StageId)
                return BadRequest();

            var updatedStage = await _stagesService.UpdateStage(stage);
            return Ok(updatedStage);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStage(int id)
        {
            var result = await _stagesService.DeleteStage(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}