using DDPApi.Models;

namespace DDPApi.Interfaces;

public interface IInventoryMovement
{
    Task<InventoryMovement> AddInventoryMovementAsync(InventoryMovement inventoryMovement); // Envanter hareketi ekleme
    Task<InventoryMovement> UpdateInventoryMovementAsync(int id, InventoryMovement inventoryMovement); // Envanter hareketini güncelleme
    Task<bool> DeleteInventoryMovementAsync(int id); // Envanter hareketini silme
    Task<InventoryMovement> GetInventoryMovementByIdAsync(int id); // Envanter hareketini ID ile sorgulama
    Task<IEnumerable<InventoryMovement>> GetAllInventoryMovementsAsync(); // Tüm envanter hareketlerini listeleme
}