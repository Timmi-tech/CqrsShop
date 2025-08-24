using Application.DTOs;
using Application.Interfaces.Services.Contracts;
using Domain.Common;
using MediatR;

namespace Application.Features.Authentication.Queries
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserPofileDto>>
    {
        private readonly IServiceManager _serviceManager;

        public GetUserByIdQueryHandler(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<Result<UserPofileDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var userProfile = await _serviceManager.UserProfileService.GetUserProfileAsync(request.UserId);
            if (userProfile == null)
                    return Result<UserPofileDto>.Failure(
                        Error.NotFound("UserProfile", request.UserId));

            return userProfile;
        }
    }
}