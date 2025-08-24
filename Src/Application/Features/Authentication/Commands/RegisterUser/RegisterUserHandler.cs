using Application.Interfaces.Services.Contracts;
using Application.Services;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Authentication.Commands.RegisterUser
{
    public class RegisterUserHandler(IServiceManager serviceManager) : IRequestHandler<RegisterUserCommand, Result>
    {
        private readonly IServiceManager _serviceManager = serviceManager;

        public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            IdentityResult identityResult = await _serviceManager.AuthenticationService.RegisterUser(request.UserForRegistration);

            if (identityResult.Succeeded)
                return Result.Success();

            var errorMessages = string.Join(", ", identityResult.Errors.Select(e => e.Description));
            return Result.Failure(Error.Validation("IdentityError", errorMessages));
        }
    }
}