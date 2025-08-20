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
            GetUserByIdQuery  query = new(userId);
            UserPofileDto userProfile = await _mediator.Send(query) ?? throw new UserProfileNotFoundException(userId);
            return Ok(userProfile);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            GetAllUserProfilesQuery query = new();
            IEnumerable<UserPofileDto> users = await _mediator.Send(query);
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