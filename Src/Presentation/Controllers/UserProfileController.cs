using Application.DTOs;
using Application.Features.Authentication.Commands.UpdateUser;
using Application.Features.Authentication.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/UserProfile")]
    public class UserProfileController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserProfile(string userId)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(userId));
            return result.Match(
                onSuccess: user => Ok(user),
                onFailure: error => StatusCode(error.StatusCode ?? 404, error)
            );
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _mediator.Send(new GetAllUserProfilesQuery());
            return result.Match(
                onSuccess: users => Ok(users),
                onFailure: error => StatusCode(error.StatusCode ?? 404, error)
            );
        }
         // PUT: api/UserProfile/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserProfile(string id, [FromBody] UserUpdateProfileDto updateDto)
        {
            await _mediator.Send(new UpdateUserProfileCommand(id, updateDto, TrackChanges: true));
            return NoContent();
        }
    }
}