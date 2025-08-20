using Application.DTOs;
using Application.Interfaces.Services.Contracts;
using MediatR;

namespace Application.Features.Authentication.Queries
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserPofileDto>
    {
        private readonly IServiceManager _serviceManager;

        public GetUserByIdQueryHandler(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<UserPofileDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var userProfile = await _serviceManager.UserProfileService.GetUserProfileAsync(request.UserId) ?? throw new UserProfileNotFoundException(request.UserId);
            return userProfile;
        }
    }
}