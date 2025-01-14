namespace DDPApi.Models;

public class InventoryMovement
{
    public int InventoryMovementId { get; set; } // Envanter hareketi için benzersiz ID
    public int ItemId { get; set; }               // Malzeme ID'si
    public string ItemName { get; set; }          // Malzeme adı
    public int Quantity { get; set; }             // Miktar
    public DateTime MovementDate { get; set; }    // Hareket tarihi
    public string MovementType { get; set; }      // Hareket türü (Giriş, Çıkış, Transfer)
    public string SourceWarehouse { get; set; }   // Kaynak depo (varsa transfer için)
    public string DestinationWarehouse { get; set; } // Hedef depo (varsa transfer için)
    public decimal Cost { get; set; }             // Birim maliyet
}