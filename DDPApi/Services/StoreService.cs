using DDPApi.Interfaces;
using DDPApi.Models;
using DDPApi.Data;
using DDPApi.DTO;
using Microsoft.EntityFrameworkCore;

namespace DDPApi.Services
{
    public class StoreService : IStore
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _companyId;

        public StoreService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            
            // JWT'den CompanyId'yi al
            var companyIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("CompanyId");
            if (companyIdClaim != null && int.TryParse(companyIdClaim.Value, out int companyId))
            {
                _companyId = companyId;
            }
        }

        public async Task<bool> AddStoreAsync(Store store)
        {
            try
            {
                store.CompanyId = _companyId;
                store.CreatedDate = DateTime.UtcNow;
                store.UpdatedDate = DateTime.UtcNow;
                await _context.Stores.AddAsync(store);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception message for better insight
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        // UpdateStoreAsync metodu
        public async Task<bool> UpdateStoreAsync(StoreDto storeDto)
        {
            try
            {
                // Veritabanından mevcut Store kaydını getir
                var existingStore = await _context.Stores
                    .Where(s => s.CompanyId == _companyId && s.StoreId == storeDto.StoreId)
                    .FirstOrDefaultAsync();
                    
                if (existingStore == null) return false;

                // DTO'dan güncellenen alanları mevcut Store kaydına uygula
                existingStore.Name = storeDto.Name;
                existingStore.Description = storeDto.Description;
                existingStore.Category = storeDto.Category;
                existingStore.Quantity = storeDto.Quantity;
                existingStore.UnitPrice = storeDto.UnitPrice;
                existingStore.Unit = storeDto.Unit;
                existingStore.Location = storeDto.Location;
                existingStore.SupplierInfo = storeDto.SupplierInfo;
                existingStore.PurchaseDate = storeDto.PurchaseDate;
                existingStore.ExpiryDate = storeDto.ExpiryDate;
                existingStore.Barcode = storeDto.Barcode;
                existingStore.SerialNumber = storeDto.SerialNumber;
                existingStore.IsActive = storeDto.IsActive;
                existingStore.MinimumStockLevel = storeDto.MinimumStockLevel;
                existingStore.MaximumStockLevel = storeDto.MaximumStockLevel;
                existingStore.Weight = storeDto.Weight;
                existingStore.Dimensions = storeDto.Dimensions;
                existingStore.StorageConditions = storeDto.StorageConditions;
                existingStore.LastInventoryDate = storeDto.LastInventoryDate;
                existingStore.QualityStatus = storeDto.QualityStatus;

                // Güncelleme tarihini ayarla
                existingStore.UpdatedDate = DateTime.UtcNow;

                // Değişiklikleri kaydet
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                // Hata durumunda false döndür
                return false;
            }
        }

        public async Task<bool> DeleteStoreAsync(int storeId)
        {
            try
            {
                var store = await _context.Stores
                    .Where(s => s.CompanyId == _companyId && s.StoreId == storeId)
                    .FirstOrDefaultAsync();
                    
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
            return await _context.Stores
                .Where(s => s.CompanyId == _companyId && s.StoreId == storeId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Store>> GetAllStoresAsync()
        {
            return await _context.Stores
                .Where(s => s.CompanyId == _companyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetActiveStoresAsync()
        {
            return await _context.Stores
                .Where(s => s.CompanyId == _companyId && s.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresByCategoryAsync(string category)
        {
            return await _context.Stores
                .Where(s => s.CompanyId == _companyId && s.Category == category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetLowStockItemsAsync()
        {
            return await _context.Stores
                .Where(s => s.CompanyId == _companyId && s.Quantity <= s.MinimumStockLevel)
                .ToListAsync();
        }

        public async Task<Store> GetStoreByBarcodeAsync(string barcode)
        {
            return await _context.Stores
                .Where(s => s.CompanyId == _companyId && s.Barcode == barcode)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Store>> GetNearExpiryItemsAsync(int daysThreshold)
        {
            var thresholdDate = DateTime.Now.AddDays(daysThreshold);
            return await _context.Stores
                .Where(s => s.CompanyId == _companyId && s.ExpiryDate.HasValue && s.ExpiryDate <= thresholdDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresByLocationAsync(string location)
        {
            return await _context.Stores
                .Where(s => s.CompanyId == _companyId && s.Location == location)
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresBySupplierAsync(string supplierInfo)
        {
            return await _context.Stores
                .Where(s => s.CompanyId == _companyId && s.SupplierInfo == supplierInfo)
                .ToListAsync();
        }

        public async Task<bool> UpdateStockQuantityAsync(int storeId, int newQuantity)
        {
            try
            {
                var store = await _context.Stores
                    .Where(s => s.CompanyId == _companyId && s.StoreId == storeId)
                    .FirstOrDefaultAsync();
                    
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
                var store = await _context.Stores
                    .Where(s => s.CompanyId == _companyId && s.StoreId == storeId)
                    .FirstOrDefaultAsync();
                    
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
                var store = await _context.Stores
                    .Where(s => s.CompanyId == _companyId && s.StoreId == storeId)
                    .FirstOrDefaultAsync();
                    
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
                .Where(s => s.CompanyId == _companyId && s.Name.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<Store>> GetStoresByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Stores
                .Where(s => s.CompanyId == _companyId && s.UnitPrice >= minPrice && s.UnitPrice <= maxPrice)
                .ToListAsync();
        }
    }
}
