using Domain.Entities.Models;

namespace Application.Interfaces.Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsByIdsAsync(IEnumerable<Guid> productIds, bool trackChanges);
        Task<IEnumerable<Product?>> GetAllProductsAsync(bool trackChanges);
        void CreateProduct(Product product);
    }
}