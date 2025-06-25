using learning_center_back.Security.Domai_.Comands;
using learning_center_back.Security.Domain.Exceptions;
using learning_center_back.Shared.Application.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace learning_center_back.Security.Presentation.REST
{
    [ApiController]
    public class UserController(IUserCommandService userCommandService) : ControllerBase
    {
        [HttpPost("sign-up")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
        {
            try
            {
                var user = await userCommandService.Handle(command);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (UsernameAlreadyTakenException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred", detail = ex.Message });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            try
            {
                var jwToken = await userCommandService.Handle(command);
                return Ok(jwToken);
            }
            catch (InvalidCredentialsException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred", detail = ex.Message });
            }
        }
    }
}