using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DDPApi.Models;
using DDPApi.Interfaces;

namespace DDPApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryMovementController : ControllerBase
    {
        private readonly IInventoryMovement _inventoryMovementService;

        public InventoryMovementController(IInventoryMovement inventoryMovementService)
        {
            _inventoryMovementService = inventoryMovementService;
        }

        // Yeni envanter hareketi ekler
        [HttpPost]
        public async Task<ActionResult<InventoryMovement>> AddInventoryMovementAsync([FromBody] InventoryMovement inventoryMovement)
        {
            if (inventoryMovement == null)
            {
                return BadRequest("InventoryMovement object cannot be null");
            }

            var addedMovement = await _inventoryMovementService.AddInventoryMovementAsync(inventoryMovement);
            return CreatedAtAction(nameof(GetInventoryMovementByIdAsync), new { id = addedMovement.InventoryMovementId }, addedMovement);
        }

        // Envanter hareketini günceller
        [HttpPut("{id}")]
        public async Task<ActionResult<InventoryMovement>> UpdateInventoryMovementAsync(int id, [FromBody] InventoryMovement inventoryMovement)
        {
            if (inventoryMovement == null)
            {
                return BadRequest("InventoryMovement object cannot be null");
            }

            var updatedMovement = await _inventoryMovementService.UpdateInventoryMovementAsync(id, inventoryMovement);
            if (updatedMovement == null)
            {
                return NotFound($"InventoryMovement with ID {id} not found");
            }

            return Ok(updatedMovement);
        }

        // Envanter hareketini siler
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInventoryMovementAsync(int id)
        {
            var result = await _inventoryMovementService.DeleteInventoryMovementAsync(id);
            if (!result)
            {
                return NotFound($"InventoryMovement with ID {id} not found");
            }

            return NoContent(); // Başarıyla silindi
        }

        // ID ile envanter hareketini getirir
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryMovement>> GetInventoryMovementByIdAsync(int id)
        {
            var movement = await _inventoryMovementService.GetInventoryMovementByIdAsync(id);
            if (movement == null)
            {
                return NotFound($"InventoryMovement with ID {id} not found");
            }

            return Ok(movement);
        }

        // Tüm envanter hareketlerini getirir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryMovement>>> GetAllInventoryMovementsAsync()
        {
            var movements = await _inventoryMovementService.GetAllInventoryMovementsAsync();
            return Ok(movements);
        }
    }
}
