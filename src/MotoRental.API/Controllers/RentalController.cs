using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using MotoRental.Application.Commands.CreateDeliveryPerson;
using MotoRental.Core.Exceptions;
using MotoRental.Application.Commands.CreateRental;
using MotoRental.Application.Queries.GetRentalById;
using MotoRental.Application.Commands.UpdateRental;

namespace MotoRental.API.Controllers
{
    [Route("api/rental")]
    [Authorize]
    public class RentalController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RentalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // api/rental
        [HttpPost]
        [AllowAnonymous]
        // [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateRental([FromBody] CreateRentalCommand command)
        { 
            try
            {
                await _mediator.Send(command);

                return Created();
            }
            catch (Exception ex) when (ex is InvalidRentalException ||
                                       ex is RentalBadRequestException ||
                                       ex is MotorcycleNotFoundException ||
                                       ex is RentalNotFoundException)
            {
                return BadRequest(ex.Message);
            }
            catch (DeliveryPersonCnhTypeException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        // api/rental/1
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRentalById(string id)
        {
            try
            {
                var getRentalByIdQuery = new GetRentalByIdQuery(id);
                var rental = await _mediator.Send(getRentalByIdQuery);

                if (rental == null)
                {
                    return NotFound();
                }

                return Ok(rental);
            }
            catch (RentalNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        // api/rental/1/return
        [HttpPut("{id}/return")]
        [AllowAnonymous]
        public async Task<ActionResult> RentalReturn(string id, [FromBody] UpdateRentalCommand command)
        {
            try
            {
                command.RentalId = id;
                await _mediator.Send(command);

                return NoContent();
            }
            catch (RentalNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

    }
}
