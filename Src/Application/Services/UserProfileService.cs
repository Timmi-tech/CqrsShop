using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Contracts;
using Application.Interfaces.Services.Contracts;

namespace Application.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public UserProfileService(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }


        public async Task<UserPofileDto> GetUserProfileAsync(string userId)
        {
            var user = await _repository.User.GetUserProfileAsync(userId, trackChanges: false);
            if (user == null)
            {
                _logger.LogError($"User with id {userId} not found.");
                throw new UserProfileNotFoundException(userId);
            }
            return new UserPofileDto
            {
                Id = user.Id,
                Firstname = user.FirstName,
                Lastname = user.LastName,
                Email = user.Email!,
                Username = user.UserName!
            };
        }
        public async Task<IEnumerable<UserPofileDto>> GetAllUserProfilesAsync()
        {
            var users = await _repository.User.GetAllUserProfilesAsync(trackChanges: false);
            return users.Select(user => new UserPofileDto
            {
                Id = user.Id,
                Firstname = user.FirstName,
                Lastname = user.LastName,
                Email = user.Email!,
                Username = user.UserName!,
                Role = user.Role.ToString()
            });
        }
       public async Task  UpdateUserProfileAsync(string userId, UserUpdateProfileDto userUpdateProfileDto, bool trackChanges)
        {
            var userUpdateEntity = await _repository.User.GetUserProfileAsync(userId, trackChanges);
            if (userUpdateEntity is null)
            {
                throw new UserProfileNotFoundException(userId);
            }
            userUpdateEntity.FirstName = userUpdateProfileDto.Firstname;
            userUpdateEntity.LastName = userUpdateProfileDto.Lastname;
            userUpdateEntity.UserName = userUpdateProfileDto.Username;

            await _repository.SaveAsync();
        }
    }
}