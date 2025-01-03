﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using MotoRental.Application.Commands.CreateDeliveryPerson;
using MotoRental.Core.Exceptions;
using MotoRental.Application.Commands.CreateRental;
using MotoRental.Application.Queries.GetRentalById;
using MotoRental.Application.Commands.UpdateRental;
using MotoRental.Application.InputModels;

namespace MotoRental.API.Controllers
{
    [Route("locacao")]
    [Authorize]
    public class RentalController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RentalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
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
                                       ex is DeliveryPersonNotFoundException ||
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

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetRentalById(string id)
        {
            try
            {
                var getRentalByIdQuery = new GetRentalByIdQuery(id);
                var rental = await _mediator.Send(getRentalByIdQuery);

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

        [HttpPut("{id}/devolucao")]
        [Authorize]
        public async Task<ActionResult> RentalReturn(string id, [FromBody] UpdateRentalInputModel inputModel)
        {
            try
            {
                var command = new UpdateRentalCommand(id, inputModel.data_devolucao);
                await _mediator.Send(command);

                return NoContent();
            }
            catch (RentalBadRequestException ex)
            {
                return BadRequest(ex.Message);
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
