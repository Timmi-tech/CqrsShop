using Application.Interfaces.Contracts;
using Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
  public class ProductRepository(RepositoryWriteDbContext repositoryReadContextFactory) : ReadRepository<Product>(repositoryReadContextFactory), IProductRepository
  {
        // Get product(s) by multiple IDs
        public async Task<IEnumerable<Product>> GetProductsByIdsAsync(IEnumerable<Guid> productIds, bool trackChanges) =>
        await FindByCondition(p => productIds.Contains(p.Id), trackChanges)
            .Include(p => p.Inventory) 
            .ToListAsync();


    //  Get all Products
    public async Task<IEnumerable<Product?>> GetAllProductsAsync(bool trackChanges) => await FindAll(trackChanges)
        .Include(p => p.Inventory)
        .ToListAsync();

    public void CreateProduct(Product product) => Create(product);
  }
    
}