﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using MotoRental.Application.Commands.CreateDeliveryPerson;
using MotoRental.Core.Exceptions;

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
                var id = await _mediator.Send(command);

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

        
    }
}