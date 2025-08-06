using Application.DTOs;
using Application.Features.Authentication.Commands.LoginUser;
using Application.Interfaces.Services.Contracts;
using MediatR;

namespace Application.Features.Authentication.Handlers
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, TokenDto>
    {
        private readonly IServiceManager _serviceManager;


        public LoginUserHandler(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public async Task<TokenDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var isValidUser = await _serviceManager.AuthenticationService.ValidateUser(request.UserForAuthentication);
            if (!isValidUser)
                throw new UnauthorizedAccessException("Invalid credentials.");

            return await _serviceManager.AuthenticationService.CreateToken(populateExp: true);
        }
    }
}
