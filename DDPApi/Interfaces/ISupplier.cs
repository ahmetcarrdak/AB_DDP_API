using DDPApi.Models;

namespace DDPApi.Interfaces;

public interface ISupplier
{
    Task<Supplier> AddSupplierAsync(Supplier supplier); // Yeni tedarikçi ekleme
    Task<Supplier> UpdateSupplierAsync(int id, Supplier supplier); // Tedarikçi bilgilerini güncelleme
    Task<bool> DeleteSupplierAsync(int id); // Tedarikçiyi silme
    Task<Supplier> GetSupplierByIdAsync(int id); // Tedarikçiyi ID ile sorgulama
    Task<IEnumerable<Supplier>> GetAllSuppliersAsync(); // Tüm tedarikçileri listeleme

}