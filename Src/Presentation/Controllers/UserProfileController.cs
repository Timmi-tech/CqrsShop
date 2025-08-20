using Application.DTOs;
using Application.Features.Authentication.Commands.UpdateUser;
using Application.Features.Authentication.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/UserProfile")]
    public class UserProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserProfile(string userId)
        {
            var query = new GetUserByIdQuery(userId);
            var userProfile = await _mediator.Send(query);

            if (userProfile == null)
            {
                throw new UserProfileNotFoundException(userId);
            }
            return Ok(userProfile);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var query = new GetAllUserProfilesQuery();
            var users = await _mediator.Send(query);
            return Ok(users);
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