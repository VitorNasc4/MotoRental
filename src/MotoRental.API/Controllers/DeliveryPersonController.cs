using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using MotoRental.Application.Commands.CreateDeliveryPerson;
using MotoRental.Core.Exceptions;
using MotoRental.Application.Commands.UploadCnhImage;

namespace MotoRental.API.Controllers
{
    [Route("api/deliveryPerson")]
    [Authorize]
    public class DeliveryPersonController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DeliveryPersonController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // api/deliveryPerson
        [HttpPost]
        [AllowAnonymous]
        // [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateDeliveryPerson([FromBody] CreateDeliveryPersonCommand command)
        { 
            try
            {
                await _mediator.Send(command);

                return Created();
            }
            catch (DeliveryPersonAlreadyExistsException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
        // api/deliveryPerson/1/cnh
        [HttpPost("{id}/cnh")]
        [AllowAnonymous]
        // [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UploadCnhImage(string id, [FromBody] UploadCnhImageCommand command)
        { 
            try
            {
                command.deliveryPersonId = id;
                await _mediator.Send(command);

                return Created();
            }
            catch (DeliveryPersonNotFoundException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }



        
    }
}
