using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DDPApi.Models;
using DDPApi.Interfaces;

namespace DDPApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplier _supplierService;

        public SupplierController(ISupplier supplierService)
        {
            _supplierService = supplierService;
        }

        // Yeni bir tedarikçi ekler
        [HttpPost]
        public async Task<ActionResult<Supplier>> AddSupplierAsync([FromBody] Supplier supplier)
        {
            if (supplier == null)
            {
                return BadRequest("Supplier cannot be null");
            }

            var addedSupplier = await _supplierService.AddSupplierAsync(supplier);
            return CreatedAtAction(nameof(GetSupplierByIdAsync), new { id = addedSupplier.SupplierId }, addedSupplier);
        }

        // Var olan tedarikçiyi günceller
        [HttpPut("{id}")]
        public async Task<ActionResult<Supplier>> UpdateSupplierAsync(int id, [FromBody] Supplier supplier)
        {
            if (supplier == null)
            {
                return BadRequest("Supplier cannot be null");
            }

            var updatedSupplier = await _supplierService.UpdateSupplierAsync(id, supplier);
            if (updatedSupplier == null)
            {
                return NotFound($"Supplier with ID {id} not found");
            }

            return Ok(updatedSupplier);
        }

        // Tedarikçiyi siler
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSupplierAsync(int id)
        {
            var result = await _supplierService.DeleteSupplierAsync(id);
            if (!result)
            {
                return NotFound($"Supplier with ID {id} not found");
            }

            return NoContent();
        }

        // ID'ye göre tedarikçiyi getirir
        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplierByIdAsync(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null)
            {
                return NotFound($"Supplier with ID {id} not found");
            }

            return Ok(supplier);
        }

        // Tüm tedarikçileri getirir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetAllSuppliersAsync()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            return Ok(suppliers);
        }
    }
}
