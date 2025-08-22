using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Contracts;
using Application.Interfaces.Services.Contracts;
using Domain.Entities.Models;

namespace Application.Services
{
    public class UserProfileService(IRepositoryManager repository, ILoggerManager logger) : IUserProfileService
    {
        private readonly IRepositoryManager _repository = repository;
        private readonly ILoggerManager _logger = logger;

        public async Task<UserPofileDto> GetUserProfileAsync(string userId)
        {
            User? user = await _repository.User.GetUserProfileAsync(userId, trackChanges: false);
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
            IEnumerable<User> users = await _repository.User.GetAllUserProfilesAsync(trackChanges: false);
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
            User userUpdateEntity = await _repository.User.GetUserProfileAsync(userId, trackChanges) ?? throw new UserProfileNotFoundException(userId);
            userUpdateEntity.FirstName = userUpdateProfileDto.Firstname;
            userUpdateEntity.LastName = userUpdateProfileDto.Lastname;
            userUpdateEntity.UserName = userUpdateProfileDto.Username;

            await _repository.SaveAsync();
        }
    }
}