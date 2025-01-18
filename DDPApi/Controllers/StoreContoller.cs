using DDPApi.DTO;
using DDPApi.Interfaces;
using DDPApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DDPApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly IStore _storeService;

        public StoreController(IStore storeService)
        {
            _storeService = storeService;
        }

        // GET: api/Store/all
        // Tüm malzemeleri listeler
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Store>>> GetAllStores()
        {
            var stores = await _storeService.GetAllStoresAsync();
            return Ok(stores);
        }

        // GET: api/Store/active
        // Aktif malzemeleri listeler
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Store>>> GetActiveStores()
        {
            var stores = await _storeService.GetActiveStoresAsync();
            return Ok(stores);
        }

        // GET: api/Store/{id}
        // ID'ye göre malzeme getirir
        [HttpGet("{id}")]
        public async Task<ActionResult<Store>> GetStore(int id)
        {
            var store = await _storeService.GetStoreByIdAsync(id);
            if (store == null)
                return NotFound();
            return Ok(store);
        }

        // POST: api/Store
        // Yeni malzeme ekler
        [HttpPost]
        public async Task<ActionResult<Store>> CreateStore(Store store)
        {
            var result = await _storeService.AddStoreAsync(store);
            if (!result)
                return BadRequest();
            return CreatedAtAction(nameof(GetStore), new { id = store.StoreId }, store);
        }

        // API endpointi
        // PUT: api/Store/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateStore(StoreDto storeDto)
        {
            var result = await _storeService.UpdateStoreAsync(storeDto);
            if (!result)
                return NotFound(new { message = "Store not found." });

            return NoContent();
        }

        // DELETE: api/Store/{id}
        // Malzeme siler
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            var result = await _storeService.DeleteStoreAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        // GET: api/Store/category/{category}
        // Kategoriye göre malzemeleri listeler
        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<Store>>> GetStoresByCategory(string category)
        {
            var stores = await _storeService.GetStoresByCategoryAsync(category);
            return Ok(stores);
        }

        // GET: api/Store/lowstock
        // Minimum stok seviyesinin altındaki malzemeleri listeler
        [HttpGet("lowstock")]
        public async Task<ActionResult<IEnumerable<Store>>> GetLowStockItems()
        {
            var stores = await _storeService.GetLowStockItemsAsync();
            return Ok(stores);
        }

        // GET: api/Store/barcode/{barcode}
        // Barkod numarasına göre malzeme arar
        [HttpGet("barcode/{barcode}")]
        public async Task<ActionResult<Store>> GetStoreByBarcode(string barcode)
        {
            var store = await _storeService.GetStoreByBarcodeAsync(barcode);
            if (store == null)
                return NotFound();
            return Ok(store);
        }

        // GET: api/Store/nearexpiry/{days}
        // Son kullanma tarihi yaklaşan malzemeleri listeler
        [HttpGet("nearexpiry/{days}")]
        public async Task<ActionResult<IEnumerable<Store>>> GetNearExpiryItems(int days)
        {
            var stores = await _storeService.GetNearExpiryItemsAsync(days);
            return Ok(stores);
        }

        // GET: api/Store/location/{location}
        // Belirli bir lokasyondaki malzemeleri listeler
        [HttpGet("location/{location}")]
        public async Task<ActionResult<IEnumerable<Store>>> GetStoresByLocation(string location)
        {
            var stores = await _storeService.GetStoresByLocationAsync(location);
            return Ok(stores);
        }

        // GET: api/Store/supplier/{supplier}
        // Tedarikçiye göre malzemeleri listeler
        [HttpGet("supplier/{supplier}")]
        public async Task<ActionResult<IEnumerable<Store>>> GetStoresBySupplier(string supplier)
        {
            var stores = await _storeService.GetStoresBySupplierAsync(supplier);
            return Ok(stores);
        }

        // PUT: api/Store/{id}/quantity/{quantity}
        // Stok miktarını günceller
        [HttpPut("{id}/quantity/{quantity}")]
        public async Task<IActionResult> UpdateStockQuantity(int id, int quantity)
        {
            var result = await _storeService.UpdateStockQuantityAsync(id, quantity);
            if (!result)
                return NotFound();
            return NoContent();
        }

        // PUT: api/Store/{id}/quality/{status}
        // Kalite durumunu günceller
        [HttpPut("{id}/quality/{status}")]
        public async Task<IActionResult> UpdateQualityStatus(int id, string status)
        {
            var result = await _storeService.UpdateQualityStatusAsync(id, status);
            if (!result)
                return NotFound();
            return NoContent();
        }

        // PUT: api/Store/{id}/inventory
        // Son sayım tarihini günceller
        [HttpPut("{id}/inventory")]
        public async Task<IActionResult> UpdateLastInventoryDate(int id)
        {
            var result = await _storeService.UpdateLastInventoryDateAsync(id, DateTime.Now);
            if (!result)
                return NotFound();
            return NoContent();
        }

        // GET: api/Store/search/{term}
        // İsme göre malzeme arar
        [HttpGet("search/{term}")]
        public async Task<ActionResult<IEnumerable<Store>>> SearchStores(string term)
        {
            var stores = await _storeService.SearchStoresByNameAsync(term);
            return Ok(stores);
        }

        // GET: api/Store/pricerange
        // Fiyat aralığına göre malzemeleri listeler
        [HttpGet("pricerange")]
        public async Task<ActionResult<IEnumerable<Store>>> GetStoresByPriceRange([FromQuery] decimal minPrice,
            [FromQuery] decimal maxPrice)
        {
            var stores = await _storeService.GetStoresByPriceRangeAsync(minPrice, maxPrice);
            return Ok(stores);
        }
    }
}