using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DDPApi.Models;
using DDPApi.Data;
using DDPApi.Interfaces;

public class InventoryMovementService : IInventoryMovement
{
    private readonly AppDbContext _context;

    public InventoryMovementService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryMovement> AddInventoryMovementAsync(InventoryMovement inventoryMovement)
    {
        _context.InventoryMovements.Add(inventoryMovement);
        await _context.SaveChangesAsync();
        return inventoryMovement;
    }

    public async Task<InventoryMovement> UpdateInventoryMovementAsync(int id, InventoryMovement inventoryMovement)
    {
        var existingMovement = await _context.InventoryMovements.FindAsync(id);
        if (existingMovement != null)
        {
            existingMovement.ItemId = inventoryMovement.ItemId;
            existingMovement.ItemName = inventoryMovement.ItemName;
            existingMovement.Quantity = inventoryMovement.Quantity;
            existingMovement.MovementDate = inventoryMovement.MovementDate;
            existingMovement.MovementType = inventoryMovement.MovementType;
            existingMovement.SourceWarehouse = inventoryMovement.SourceWarehouse;
            existingMovement.DestinationWarehouse = inventoryMovement.DestinationWarehouse;
            existingMovement.Cost = inventoryMovement.Cost;

            await _context.SaveChangesAsync();
        }
        return existingMovement;
    }

    public async Task<bool> DeleteInventoryMovementAsync(int id)
    {
        var movement = await _context.InventoryMovements.FindAsync(id);
        if (movement != null)
        {
            _context.InventoryMovements.Remove(movement);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<InventoryMovement> GetInventoryMovementByIdAsync(int id)
    {
        return await _context.InventoryMovements.FindAsync(id);
    }

    public async Task<IEnumerable<InventoryMovement>> GetAllInventoryMovementsAsync()
    {
        return await _context.InventoryMovements.ToListAsync();
    }
}
