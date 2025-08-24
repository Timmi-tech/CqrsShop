

using Application.DTOs;
using Domain.Common;

namespace Application.Interfaces.Services.Contracts
{
    public interface IUserProfileService
    {
        Task<Result<UserPofileDto>> GetUserProfileAsync(string userId);
        Task<Result<IEnumerable<UserPofileDto>>> GetAllUserProfilesAsync();
        Task<Result> UpdateUserProfileAsync(string userId, UserUpdateProfileDto userUpdateProfileDto, bool trackChanges);
    }
}