using Application.DTOs;
using Application.Interfaces.Services.Contracts;
using Domain.Common;
using MediatR;

namespace Application.Features.Authentication.Queries
{
    public class GetAllUserProfilesQueryHandler(IServiceManager serviceManager) : IRequestHandler<GetAllUserProfilesQuery, Result<IEnumerable<UserPofileDto>>>
    {
        private readonly IServiceManager _serviceManager = serviceManager;

        public async Task<Result<IEnumerable<UserPofileDto>>> Handle(GetAllUserProfilesQuery request, CancellationToken cancellationToken)
        {
            var userProfiles = await _serviceManager.UserProfileService.GetAllUserProfilesAsync();
            return userProfiles;
        }
    }
}