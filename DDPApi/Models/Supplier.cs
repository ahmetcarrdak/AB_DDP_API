namespace DDPApi.Models;

public class Supplier
{
    public int SupplierId { get; set; } // Tedarikçi ID'si
    public string Name { get; set; } // Firma adı
    public string ContactPerson { get; set; } // İletişim kişisi
    public string PhoneNumber { get; set; } // Telefon numarası
    public string Email { get; set; } // E-posta adresi
    public string Address { get; set; } // Adres
    public List<SupplierProduct> SuppliedProducts { get; set; } // Tedarik edilen ürünler
    public decimal Rating { get; set; } // Tedarikçi puanlaması
}

public class SupplierProduct
{
    public int SupplierProductId { get; set; } // Ürün ID'si
    public int SupplierId { get; set; } // Tedarikçi ID'si
    public string ProductName { get; set; } // Ürün adı
    public decimal Price { get; set; } // Ürün birim fiyatı
    public int LeadTimeInDays { get; set; } // Teslimat süresi (gün)
}