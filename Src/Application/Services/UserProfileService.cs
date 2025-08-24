using Application.DTOs;
using Application.Interfaces;
using Application.Interfaces.Contracts;
using Application.Interfaces.Services.Contracts;
using Domain.Common;
using Domain.Entities.Models;

namespace Application.Services
{
    public class UserProfileService(IRepositoryManager repository, ILoggerManager logger) : IUserProfileService
    {
        private readonly IRepositoryManager _repository = repository;
        private readonly ILoggerManager _logger = logger;

        public async Task<Result<UserPofileDto>> GetUserProfileAsync(string userId)
        {
            User? user = await _repository.User.GetUserProfileAsync(userId, trackChanges: false);
            if (user == null)
            {
                _logger.LogError($"User with id {userId} not found.");
                return Result<UserPofileDto>.Failure(Error.NotFound("UserProfile", userId));
            }
            UserPofileDto dto = new()
            {
                Id = user.Id,
                Firstname = user.FirstName,
                Lastname = user.LastName,
                Email = user.Email!,
                Username = user.UserName!
            };
            return Result<UserPofileDto>.Success(dto);  
            }
        public async Task<Result<IEnumerable<UserPofileDto>>> GetAllUserProfilesAsync()
        {
            IEnumerable<User> users = await _repository.User.GetAllUserProfilesAsync(trackChanges: false);
            var dtos = users.Select(user => new UserPofileDto
            {
                Id = user.Id,
                Firstname = user.FirstName,
                Lastname = user.LastName,
                Email = user.Email!,
                Username = user.UserName!,
                Role = user.Role.ToString()
            });
            return Result<IEnumerable<UserPofileDto>>.Success(dtos);
        }
        public async Task<Result> UpdateUserProfileAsync(string userId, UserUpdateProfileDto userUpdateProfileDto, bool trackChanges)
        {
            User? userUpdateEntity = await _repository.User.GetUserProfileAsync(userId, trackChanges);
            if (userUpdateEntity == null)
            {
                _logger.LogError($"User with id {userId} not found.");
                return Result.Failure(Error.NotFound("UserProfile", userId));
            }
            userUpdateEntity.FirstName = userUpdateProfileDto.Firstname;
            userUpdateEntity.LastName = userUpdateProfileDto.Lastname;
            userUpdateEntity.UserName = userUpdateProfileDto.Username;

            await _repository.SaveAsync();
            return Result.Success();
        }
    }
}