using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Authentication.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<IdentityResult>
    {
        public UserForRegistrationDto UserForRegistration { get; init; }

    public RegisterUserCommand(UserForRegistrationDto userForRegistration)
    {
        UserForRegistration = userForRegistration;
    } 
    }
}