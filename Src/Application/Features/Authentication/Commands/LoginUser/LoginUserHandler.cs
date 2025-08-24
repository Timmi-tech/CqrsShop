using Application.DTOs;
using Application.Features.Authentication.Commands.LoginUser;
using Application.Interfaces.Services.Contracts;
using Domain.Common;
using MediatR;

namespace Application.Features.Authentication.Handlers
{
    public class LoginUserHandler(IServiceManager serviceManager) : IRequestHandler<LoginUserCommand, Result<TokenDto>>
    {
        private readonly IServiceManager _serviceManager = serviceManager;

        public async Task<Result<TokenDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var isValidUser = await _serviceManager.AuthenticationService.ValidateUser(request.UserForAuthentication);

            if (isValidUser is null)
            {
                return Result<TokenDto>.Failure(
                    Error.Validation("InvalidCredentials", "Email or password is incorrect"));
            }

            var token = await _serviceManager.AuthenticationService.CreateToken(isValidUser, populateExp: true);
            return Result<TokenDto>.Success(token);
        }
    }
}
