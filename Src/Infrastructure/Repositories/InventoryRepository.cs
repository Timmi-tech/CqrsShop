using Application.Interfaces.Contracts;
using Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class InventoryRepository(RepositoryWriteDbContext repositoryReadContextFactory) : ReadRepository<Inventory>(repositoryReadContextFactory), IInventoryRepository
    {

        public void CreateInventory(Inventory inventory) => Create(inventory);

        public async Task<Inventory?> GetInventoryByProductIdAsync(Guid productId, bool trackChanges)
        {
            return await FindByCondition(x => x.ProductId.Equals(productId), trackChanges)
             .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Inventory>> GetAllInventoriesAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .Include(i => i.Product)
                .ToListAsync();

        public async Task AdjustStockAsync(Guid productId, int quantityChange, bool trackChanges)
        {
            Inventory inventory = await GetInventoryByProductIdAsync(productId, trackChanges)
                ?? throw new KeyNotFoundException($"Inventory not found for ProductId: {productId}");

            inventory.AdjustStock(quantityChange);

            Update(inventory);
        }
    }
}