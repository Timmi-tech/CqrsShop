using Application.DTOs;
using Application.Features.Authentication.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Application.Features.Authentication.Commands.LoginUser;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/Authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new RegisterUserCommand(dto);
            IdentityResult result = await _mediator.Send(command);

            if (result.Succeeded)
            {
                return Ok(new { Message = "User registered successfully." });
            }

            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new { Errors = errors });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userDto)
        {
            var command = new LoginUserCommand(userDto);
            var token = await _mediator.Send(command);

            return Ok(token); // TokenDto is returned
        }
            
    }
}
