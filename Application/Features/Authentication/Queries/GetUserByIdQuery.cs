using Application.DTOs;
using MediatR;

namespace Application.Features.Authentication.Queries
{
    public record GetUserByIdQuery(string UserId) : IRequest<UserPofileDto>;
}