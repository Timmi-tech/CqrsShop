using Domain.Entities.Models;

namespace Application.Interfaces.Contracts
{
    public interface IProductRepository
    {
        Task<Product?> GetProductByIdAsync(Guid id, bool trackChanges);
        Task<IEnumerable<Product?>> GetAllProductsAsync(bool trackChanges);
        void CreateProduct(Product product);

    }
}