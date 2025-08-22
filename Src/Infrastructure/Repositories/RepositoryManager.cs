using Application.Interfaces.Contracts;

namespace Infrastructure.Repositories
{
    public class RepositoryManager(RepositoryWriteDbContext repositoryWriteDbContext) : IRepositoryManager
    {
        private readonly RepositoryWriteDbContext _repositoryWriteDbContext = repositoryWriteDbContext;
        private readonly Lazy<IUserProfileRepository> _userProfileRepository = new(() => new UserProfileRepository(repositoryWriteDbContext));
        private readonly Lazy<IProductRepository> _productRepository = new(() => new ProductRepository(repositoryWriteDbContext));
        private readonly Lazy<IOrderRepository> _orderRepository = new(() => new OrderRepository(repositoryWriteDbContext));
        private readonly Lazy<IInventoryRepository> _inventoryRepository = new(() => new InventoryRepository(repositoryWriteDbContext));

        public IUserProfileRepository User => _userProfileRepository.Value;
        public IProductRepository Product => _productRepository.Value;
        public IOrderRepository Order => _orderRepository.Value;
        public IInventoryRepository Inventory => _inventoryRepository.Value;

        public async Task SaveAsync()
        {
            await _repositoryWriteDbContext.SaveChangesAsync();
        }
    }
}