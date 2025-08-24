using Application.DTOs;
using Domain.Common;
using MediatR;

namespace Application.Features.Authentication.Queries
{
    public record GetAllUserProfilesQuery : IRequest<Result<IEnumerable<UserPofileDto>>>;
}