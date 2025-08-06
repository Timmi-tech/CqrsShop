using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Authentication.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<TokenDto>
    {
        public UserForAuthenticationDto UserForAuthentication { get; init; }
    public LoginUserCommand(UserForAuthenticationDto userForAuthentication)
        {
            UserForAuthentication = userForAuthentication;
        }
    }
}