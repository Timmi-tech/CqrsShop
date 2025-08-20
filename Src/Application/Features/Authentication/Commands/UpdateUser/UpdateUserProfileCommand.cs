using Application.DTOs;
using MediatR;

namespace Application.Features.Authentication.Commands.UpdateUser
{
    public record UpdateUserProfileCommand(string UserId, UserUpdateProfileDto UpdateDto, bool TrackChanges)
        : IRequest<Unit>;
}