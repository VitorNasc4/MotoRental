using MotoRental.Application.Commands.LoginUser;
using MotoRental.Application.Commands.CreateUser;
using MotoRental.Application.Queries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.API.Controllers
{
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // api/users/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var getUserQuery = new GetUserQuery(id);
            var user = await _mediator.Send(getUserQuery);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // api/users
        [HttpPost]
        [AllowAnonymous]
        public IActionResult CreateUser([FromBody] CreateUserCommand command)
        {
            var id = _mediator.Send(command);

            return CreatedAtAction(nameof(GetUserById), new { id = id }, command);
        }
        // api/users/admin
        [HttpPost("admin")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateUserAdmin([FromBody] CreateUserAdminCommand command)
        {
            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetUserById), new { id = id }, command);
        }

        // api/users/1/login
        [HttpPut("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginUserCommand command)
        {
            var loginUserViewModel = await _mediator.Send(command);

            if (loginUserViewModel is null)
            {
                return BadRequest();
            }

            return Ok(loginUserViewModel);
        }
    }
}
