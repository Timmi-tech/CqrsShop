

using Application.DTOs;

namespace Application.Interfaces.Services.Contracts
{
    public interface IUserProfileService
    {
        Task<UserPofileDto> GetUserProfileAsync(string userId);
        Task<IEnumerable<UserPofileDto>> GetAllUserProfilesAsync();
        Task UpdateUserProfileAsync(string userId, UserUpdateProfileDto userUpdateProfileDto, bool trackChanges);
    }
}