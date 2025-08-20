using Application.DTOs;
using MediatR;

namespace Application.Features.Authentication.Queries
{
    public record GetAllUserProfilesQuery : IRequest<IEnumerable<UserPofileDto>>;
}