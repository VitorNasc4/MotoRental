﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using MotoRental.Application.Queries.GetMotorcycleById;
using MotoRental.Application.Commands.CreateMotorcycle;
using MotoRental.Application.Queries.GetMotorcycleByPlate;
using MotoRental.Core.Exceptions;
using MotoRental.Application.Commands.UpdateMotorcycle;
using MotoRental.Application.Commands.DeleteMotorcycle;
using MotoRental.Core.Entities;
using MotoRental.Application.InputModels;

namespace MotoRental.API.Controllers
{
    [Route("motos")]
    [Authorize]
    public class MotorcyclesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MotorcyclesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = RoleTypes.Admin)]
        public async Task<IActionResult> GetMotorcycleById(string id)
        {
            try
            {
                var getMotorcycleByIdQuery = new GetMotorcycleByIdQuery(id);
                var motorcylce = await _mediator.Send(getMotorcycleByIdQuery);

                if (motorcylce == null)
                {
                    return NotFound();
                }

                return Ok(motorcylce);
            }
            catch (MotorcycleNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet("placa")]
        [Authorize(Roles = RoleTypes.Admin)]
        public async Task<IActionResult> GetMotorcycleByPlate([FromQuery] string plate)
        {
            try
            {
                var getMotorcyclesByPlateQuery = new GetMotorcyclesByPlateQuery(plate);
                var motorcylces = await _mediator.Send(getMotorcyclesByPlateQuery);

                if (motorcylces.Count == 0)
                {
                    return NoContent();
                }

                return Ok(motorcylces);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = RoleTypes.Admin)]
        public async Task<IActionResult> CreateMotorcycle([FromBody] CreateMotorcycleCommand command)
        { 
            try
            {
                await _mediator.Send(command);

                return Created();
            }
            catch (MotorcycleAlreadyExistsException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPut("{id}/placa")]
        [Authorize(Roles = RoleTypes.Admin)]
        public async Task<ActionResult> ChangePlate(string id, [FromBody] UpdateMotorcycleInputModel inputModel)
        {
            try
            {
                var command = new UpdateMotorcycleCommand(id, inputModel.placa);
                
                await _mediator.Send(command);

                return NoContent();
            }
            catch (MotorcycleAlreadyExistsException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (MotorcycleNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = RoleTypes.Admin)]
        public async Task<IActionResult> DeleteMotorcycle(string id)
        {
            try
            {
                var command = new DeleteMotorcycleCommand(id);
                await _mediator.Send(command);

                return NoContent();
            }
            catch (MotorcycleNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (MotorcycleRentalHistoricFoundException ex)
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
