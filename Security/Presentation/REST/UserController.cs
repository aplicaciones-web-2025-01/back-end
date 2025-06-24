using learning_center_back.Security.Domai_.Comands;
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
            var user = await userCommandService.Handle(command);
            return StatusCode(StatusCodes.Status201Created, user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var user = await userCommandService.Handle(command);
            return Ok(user);
        }

    }
}