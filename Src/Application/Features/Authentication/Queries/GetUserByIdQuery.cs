using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.Features.Authentication.Queries
{
    public record GetUserByIdQuery(string UserId) : IRequest<Result<UserPofileDto>>;
}