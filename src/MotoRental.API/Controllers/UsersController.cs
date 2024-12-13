using MotoRental.Application.Commands.LoginUser;
using MotoRental.Application.Commands.CreateUser;
using MotoRental.Application.Queries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using MotoRental.Application.InputModels;
using MotoRental.Application.Commands.UpdateUserToAdmin;
using MotoRental.Core.Entities;
using System;
using MotoRental.Core.Exceptions;

namespace MotoRental.API.Controllers
{
    [Route("usuarios")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var getUserQuery = new GetUserQuery(id);
                var user = await _mediator.Send(getUserQuery);

                return Ok(user);
            }
            catch (UserIdNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            try
            {
                var id = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetUserById), new { id = id }, command);
            }
            catch (EmailAlreadyExistsException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPut("{id}/admin")]
        [Authorize(Roles = RoleTypes.Admin)]
        public async Task<IActionResult> UpdateUserToAdmin(string id, [FromBody] UpdateUserInputModel inputModel)
        {
            try
            {
                var command = new UpdateUserToAdminCommand(id, inputModel.isAdmin);

                await _mediator.Send(command);

                return NoContent();
            }
            catch (UserIdNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPut("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginUserCommand command)
        {
            try
            {
                var loginUserViewModel = await _mediator.Send(command);

                return Ok(loginUserViewModel);
            }
            catch (UserNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}
