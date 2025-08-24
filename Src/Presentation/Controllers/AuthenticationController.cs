using Application.DTOs;
using Application.Features.Authentication.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Application.Features.Authentication.Commands.LoginUser;
using Domain.Common;

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
                return BadRequest(Result.Failure(
                    Error.Validation("Invalid Model", "The registration request is invalid.")));
            }

            RegisterUserCommand command = new(dto);
            Result result = await _mediator.Send(command);

            if (result.IsSuccess)
                return Ok(new { Message = "User registered successfully." });

            return BadRequest(result.Error);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userDto)
        {
            LoginUserCommand command = new(userDto);
            var result = await _mediator.Send(command);

            return result.Match<IActionResult>(
                token => Ok(token),
                error => Unauthorized(error)
                );

        }
    }
}
