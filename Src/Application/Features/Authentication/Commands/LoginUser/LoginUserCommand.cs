using Application.DTOs;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Authentication.Commands.LoginUser
{
    public class LoginUserCommand(UserForAuthenticationDto userForAuthentication) : IRequest<Result<TokenDto>>
    {
        public UserForAuthenticationDto UserForAuthentication { get; init; } = userForAuthentication;
    }
}