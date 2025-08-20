using Domain.Entities.Models;

namespace Application.Interfaces.Contracts
{
    public interface IInventoryRepository
    {
        void CreateInventory(Inventory inventory);
        Task<Inventory?> GetInventoryByProductIdAsync(Guid productId, bool trackChanges);
        Task AdjustStockAsync(Guid productId, int quantityChange, bool trackChanges);
    }
}