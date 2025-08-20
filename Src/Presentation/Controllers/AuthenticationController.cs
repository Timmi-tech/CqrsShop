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
    public class AuthenticationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return Ok(BadRequest(ModelState));
            }

            RegisterUserCommand command = new(dto);
            IdentityResult result = await _mediator.Send(command);

            if (result.Succeeded)
            {
                return Ok(new { Message = "User registered successfully." });
            }

            IEnumerable<string> errors = result.Errors.Select(e => e.Description);
            return BadRequest(new { Errors = errors });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userDto)
        {
            LoginUserCommand command = new(userDto);
            var token = await _mediator.Send(command);

            return Ok(token); // TokenDto is returned
        }
            
    }
}
