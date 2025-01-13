using System;

namespace DDPApi.Models
{
    public class Store
    {
        public int StoreId { get; set; }  // Malzeme benzersiz kimlik numarası
        public string Name { get; set; } // Malzeme adı
        public string? Description { get; set; } // Malzeme açıklaması (nullable)
        public string? Category { get; set; } // Malzeme kategorisi (nullable)
        public int Quantity { get; set; } // Stok miktarı
        public decimal UnitPrice { get; set; } // Birim fiyatı
        public string Unit { get; set; } // Ölçü birimi (kg, adet, metre vb.)
        public string? Location { get; set; } // Depo içindeki konumu/raf bilgisi (nullable)
        public string? SupplierInfo { get; set; } // Tedarikçi bilgisi (nullable)
        public DateTime PurchaseDate { get; set; } // Satın alma tarihi
        public DateTime? ExpiryDate { get; set; } // Son kullanma tarihi (nullable)
        public string? Barcode { get; set; } // Barkod numarası (nullable)
        public string? SerialNumber { get; set; } // Seri numarası (nullable)
        public bool IsActive { get; set; } // Aktif/Pasif durumu
        public int MinimumStockLevel { get; set; } // Minimum stok seviyesi
        public int MaximumStockLevel { get; set; } // Maksimum stok seviyesi
        public decimal Weight { get; set; } // Ağırlık
        public string? Dimensions { get; set; } // Boyutlar (en x boy x yükseklik) (nullable)
        public string? StorageConditions { get; set; } // Depolama koşulları (nullable)
        public DateTime LastInventoryDate { get; set; } // Son sayım tarihi
        public string? QualityStatus { get; set; } // Kalite durumu (nullable)
        public DateTime CreatedDate { get; set; } // Kayıt oluşturulma tarihi
        public DateTime UpdatedDate { get; set; } // Son güncelleme tarihi
    }
}