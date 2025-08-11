using Application.Interfaces.Contracts;
using Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class ProductRepository : ReadRepository<Product>, IProductRepository
    {
        public ProductRepository(RepositoryWriteDbContext repositoryReadContextFactory) : base(repositoryReadContextFactory)
        {
        }

        // Get product by id
        public async Task<Product?> GetProductByIdAsync(Guid productId, bool trackChanges) => await FindByCondition(c => c.Id.Equals(productId), trackChanges)
          .FirstOrDefaultAsync();

        //  Get all Products
        public async Task<IEnumerable<Product?>> GetAllProductsAsync(bool trackChanges) => await FindAll(trackChanges)
          .ToListAsync();

        public void CreateProduct(Product product) => Create(product);
    }
    
}