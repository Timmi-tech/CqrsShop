using Application.DTOs;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Authentication.Commands.RegisterUser
{
    public class RegisterUserCommand(UserForRegistrationDto userForRegistration) : IRequest<Result>
    {
        public UserForRegistrationDto UserForRegistration { get; init; } = userForRegistration;
    }
}