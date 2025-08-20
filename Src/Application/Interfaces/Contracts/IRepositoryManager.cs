namespace Application.Interfaces.Contracts
{
    public interface IRepositoryManager
    {
        IUserProfileRepository User { get; }
        IProductRepository Product { get; }
        IOrderRepository Order { get; }
        IInventoryRepository Inventory { get; }
        Task SaveAsync();
    }
}