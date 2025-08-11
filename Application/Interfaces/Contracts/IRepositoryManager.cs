namespace Application.Interfaces.Contracts
{
    public interface IRepositoryManager
    {
        IUserProfileRepository User { get; }
        IProductRepository Product { get; }
        Task SaveAsync();
    }
}