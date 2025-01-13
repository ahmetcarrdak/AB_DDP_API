using DDPApi.Models;
namespace DDPApi.Interfaces;

public interface IStore
{
    // Malzeme ekleme işlemi
    Task<bool> AddStoreAsync(Store store);

    // Malzeme güncelleme işlemi
    Task<bool> UpdateStoreAsync(Store store);

    // Malzeme silme işlemi
    Task<bool> DeleteStoreAsync(int storeId);

    // ID'ye göre malzeme getirme
    Task<Store> GetStoreByIdAsync(int storeId);

    // Tüm malzemeleri listeleme
    Task<IEnumerable<Store>> GetAllStoresAsync();

    // Aktif malzemeleri listeleme
    Task<IEnumerable<Store>> GetActiveStoresAsync();

    // Kategori bazlı malzeme listeleme
    Task<IEnumerable<Store>> GetStoresByCategoryAsync(string category);

    // Minimum stok seviyesinin altındaki malzemeleri listeleme
    Task<IEnumerable<Store>> GetLowStockItemsAsync();

    // Barkod numarasına göre malzeme arama
    Task<Store> GetStoreByBarcodeAsync(string barcode);

    // Son kullanma tarihi yaklaşan malzemeleri listeleme
    Task<IEnumerable<Store>> GetNearExpiryItemsAsync(int daysThreshold);

    // Belirli bir lokasyondaki malzemeleri listeleme
    Task<IEnumerable<Store>> GetStoresByLocationAsync(string location);

    // Tedarikçiye göre malzeme listeleme
    Task<IEnumerable<Store>> GetStoresBySupplierAsync(string supplierInfo);

    // Stok miktarı güncelleme işlemi
    Task<bool> UpdateStockQuantityAsync(int storeId, int newQuantity);

    // Kalite durumu güncelleme işlemi
    Task<bool> UpdateQualityStatusAsync(int storeId, string qualityStatus);

    // Son sayım tarihini güncelleme
    Task<bool> UpdateLastInventoryDateAsync(int storeId, DateTime lastInventoryDate);

    // İsme göre malzeme arama
    Task<IEnumerable<Store>> SearchStoresByNameAsync(string searchTerm);

    // Fiyat aralığına göre malzeme listeleme
    Task<IEnumerable<Store>> GetStoresByPriceRangeAsync(decimal minPrice, decimal maxPrice);
}