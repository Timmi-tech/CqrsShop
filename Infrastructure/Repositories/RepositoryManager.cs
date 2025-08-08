using Application.Interfaces.Contracts;

namespace Infrastructure.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryWriteDbContext _repositoryWriteDbContext;
        private readonly Lazy<IUserProfileRepository> _userProfileRepository;


        public RepositoryManager(RepositoryWriteDbContext repositoryWriteDbContext)
        {
            _repositoryWriteDbContext = repositoryWriteDbContext;
            _userProfileRepository = new Lazy<IUserProfileRepository>(() => new UserProfileRepository(repositoryWriteDbContext));
        }
        public IUserProfileRepository User => _userProfileRepository.Value;

        public async Task SaveAsync()
        {
            await _repositoryWriteDbContext.SaveChangesAsync();
        }
    }
}