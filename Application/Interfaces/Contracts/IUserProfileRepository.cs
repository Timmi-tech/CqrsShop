using Domain.Entities.Models;

namespace Application.Interfaces.Contracts
{
    public interface IUserProfileRepository
    {
        Task<User?> GetUserProfileAsync(string userId, bool trackChanges); 
        Task<IEnumerable<User>> GetAllUserProfilesAsync(bool trackChanges); 
          
    }
}