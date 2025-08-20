using Application.Interfaces.Services.Contracts;
using Application.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Authentication.Commands.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, IdentityResult>
    {
       private readonly IServiceManager _serviceManager;

        public RegisterUserHandler(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _serviceManager.AuthenticationService.RegisterUser(request.UserForRegistration);
            return result;
        }
    }
}