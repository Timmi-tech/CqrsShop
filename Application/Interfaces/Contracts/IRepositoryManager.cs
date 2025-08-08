namespace Application.Interfaces.Contracts
{
    public interface IRepositoryManager
    {
        IUserProfileRepository User { get; }
        Task SaveAsync();
    }
}