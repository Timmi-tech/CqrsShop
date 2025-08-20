using Application.Interfaces.Services.Contracts;
using MediatR;

namespace Application.Features.Authentication.Commands.UpdateUser
{
    public class UpdateUserProfileHandler : IRequestHandler<UpdateUserProfileCommand, Unit>
    {
        private readonly IServiceManager _serviceManager;

        public UpdateUserProfileHandler(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<Unit> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            await _serviceManager.UserProfileService.UpdateUserProfileAsync(
                request.UserId,
                request.UpdateDto,
                trackChanges: true);

            return Unit.Value; 
        }
    }
}
