using Application.Interfaces.Contracts;

namespace Infrastructure.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryWriteDbContext _repositoryWriteDbContext;
        private readonly Lazy<IUserProfileRepository> _userProfileRepository;
        private readonly Lazy<IProductRepository> _productRepository;
        private readonly Lazy<IOrderRepository> _orderRepository;
        private readonly Lazy<IInventoryRepository> _inventoryRepository;


        public RepositoryManager(RepositoryWriteDbContext repositoryWriteDbContext)
        {
            _repositoryWriteDbContext = repositoryWriteDbContext;
            _userProfileRepository = new Lazy<IUserProfileRepository>(() => new UserProfileRepository(repositoryWriteDbContext));
            _productRepository = new Lazy<IProductRepository>(() => new ProductRepository(repositoryWriteDbContext));
            _orderRepository = new Lazy<IOrderRepository>(() => new OrderRepository(repositoryWriteDbContext));
            _inventoryRepository = new Lazy<IInventoryRepository>(() => new InventoryRepository(repositoryWriteDbContext));
        }
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