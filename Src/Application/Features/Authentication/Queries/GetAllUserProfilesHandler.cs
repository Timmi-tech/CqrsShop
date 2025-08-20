using Application.DTOs;
using Application.Interfaces.Services.Contracts;
using MediatR;

namespace Application.Features.Authentication.Queries
{
    public class GetAllUserProfilesQueryHandler : IRequestHandler<GetAllUserProfilesQuery, IEnumerable<UserPofileDto>>
    {
        private readonly IServiceManager _serviceManager;

        public GetAllUserProfilesQueryHandler(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<IEnumerable<UserPofileDto>> Handle(GetAllUserProfilesQuery request, CancellationToken cancellationToken)
        {
            var userProfiles = await _serviceManager.UserProfileService.GetAllUserProfilesAsync();
            return userProfiles;
        }
    }
}