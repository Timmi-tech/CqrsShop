using Domain.Entities.Models;

namespace Application.Interfaces.Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsByIdsAsync(IEnumerable<Guid> productIds, bool trackChanges);
        Task<IEnumerable<Product?>> GetAllProductsAsync(bool trackChanges);
        Task<(IEnumerable<Product>, int)> GetProductsPagedAsync(int pageNumber, int pageSize, bool trackChanges);
        void CreateProduct(Product product);
        void DeleteProduct(Product product);
    }
}