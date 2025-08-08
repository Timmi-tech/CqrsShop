using Application.Interfaces.Contracts;
using Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserProfileRepository : ReadRepository<User>, IUserProfileRepository
    {
        public UserProfileRepository(RepositoryWriteDbContext repositoryReadContextFactory)
            : base(repositoryReadContextFactory)
        {
        }

        public async Task<User?> GetUserProfileAsync(string userId, bool trackChanges) => await FindByCondition(c => c.Id.Equals(userId), trackChanges).SingleOrDefaultAsync();

        public async Task<IEnumerable<User>> GetAllUserProfilesAsync(bool trackChanges) =>
            await FindAll(trackChanges).ToListAsync();
    }
}