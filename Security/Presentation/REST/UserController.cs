using learning_center_back.Security.Domai_.Comands;
using learning_center_back.Security.Domain.Exceptions;
using learning_center_back.Shared.Application.Commands;
using Microsoft.AspNetCore.Mvc;

namespace learning_center_back.Security.Presentation.REST
{
    [ApiController]
    public class UserController(IUserCommandService userCommandService) : ControllerBase
    {
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
        {
            try
            {
                var user = await userCommandService.Handle(command);
                return StatusCode(StatusCodes.Status201Created, user);
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
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            try
            {
                var user = await userCommandService.Handle(command);
                return Ok(user);
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