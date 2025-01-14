using DDPApi.Interfaces;
using DDPApi.Models;
using DDPApi.Data;
using Microsoft.EntityFrameworkCore;

namespace DDPApi.Services
{
    public class StoreService : IStore
    {
        private readonly AppDbContext _context;

        public StoreService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddStoreAsync(Store store)
        {
            try
            {
                store.CreatedDate = DateTime.Now;
                store.UpdatedDate = DateTime.Now;
                await _context.Stores.AddAsync(store);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateStoreAsync(Store store)
        {
            try
            {
                var existingStore = await _context.Stores.FindAsync(store.StoreId);
                if (existingStore == null) return false;

                store.UpdatedDate = DateTime.Now;
                _context.Entry(existingStore).CurrentValues.SetValues(store);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteStoreAsync(int storeId)
        {
            try
            {
                var store = await _context.Stores.FindAsync(storeId);
                if (store == null) return false;

                _context.Stores.Remove(store);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Store> GetStoreByIdAsync(int storeId)
        {
            return await _context.Stores.FindAsync(storeId);
        }

        public async Task<IEnumerable<Store>> GetAllStoresAsync()
        {
            return await _context.Stores.ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetActiveStoresAsync()
        {
            return await _context.Stores.Where(s => s.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresByCategoryAsync(string category)
        {
            return await _context.Stores.Where(s => s.Category == category).ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetLowStockItemsAsync()
        {
            return await _context.Stores.Where(s => s.Quantity <= s.MinimumStockLevel).ToListAsync();
        }

        public async Task<Store> GetStoreByBarcodeAsync(string barcode)
        {
            return await _context.Stores.FirstOrDefaultAsync(s => s.Barcode == barcode);
        }

        public async Task<IEnumerable<Store>> GetNearExpiryItemsAsync(int daysThreshold)
        {
            var thresholdDate = DateTime.Now.AddDays(daysThreshold);
            return await _context.Stores
                .Where(s => s.ExpiryDate.HasValue && s.ExpiryDate <= thresholdDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresByLocationAsync(string location)
        {
            return await _context.Stores.Where(s => s.Location == location).ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresBySupplierAsync(string supplierInfo)
        {
            return await _context.Stores.Where(s => s.SupplierInfo == supplierInfo).ToListAsync();
        }

        public async Task<bool> UpdateStockQuantityAsync(int storeId, int newQuantity)
        {
            try
            {
                var store = await _context.Stores.FindAsync(storeId);
                if (store == null) return false;

                store.Quantity = newQuantity;
                store.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateQualityStatusAsync(int storeId, string qualityStatus)
        {
            try
            {
                var store = await _context.Stores.FindAsync(storeId);
                if (store == null) return false;

                store.QualityStatus = qualityStatus;
                store.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateLastInventoryDateAsync(int storeId, DateTime lastInventoryDate)
        {
            try
            {
                var store = await _context.Stores.FindAsync(storeId);
                if (store == null) return false;

                store.LastInventoryDate = lastInventoryDate;
                store.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Store>> SearchStoresByNameAsync(string searchTerm)
        {
            return await _context.Stores
                .Where(s => s.Name.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Stores
                .Where(s => s.UnitPrice >= minPrice && s.UnitPrice <= maxPrice)
                .ToListAsync();
        }
    }
}
