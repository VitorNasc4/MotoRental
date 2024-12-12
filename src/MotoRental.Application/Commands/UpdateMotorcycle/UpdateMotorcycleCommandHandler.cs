using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MotoRental.Core.Entities;
using MotoRental.Core.Repositories;
using MotoRental.Core.Services;
using MotoRental.Infrastructure.Persistence;
using MediatR;
using MotoRental.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace MotoRental.Application.Commands.UpdateMotorcycle
{
    public class UpdateMotorcycleCommandHandler : IRequestHandler<UpdateMotorcycleCommand>
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly ILogger<UpdateMotorcycleCommandHandler> _logger;

        public UpdateMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository, ILogger<UpdateMotorcycleCommandHandler> logger)
        {
            _motorcycleRepository = motorcycleRepository;
            _logger = logger;
        }
        public async Task<Unit> Handle(UpdateMotorcycleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"Iniciando processo de atualização de placa. Id da moto: {request.MotorcycleId}, Placa: {request.NewPlate}");
            
            var motorcycleById = await _motorcycleRepository.GetMotorcycleByIdAsync(request.MotorcycleId);

            if (motorcycleById is null)
            {
                _logger.LogError($"Interrompendo processo de atualização de placa. Motivo: Registro de moto não encontrado. Id buscado: {request.MotorcycleId}");
                throw new MotorcycleNotFoundException(request.MotorcycleId);
            }

            var motorcyclePlateAlreadyExist = await _motorcycleRepository.MotorcyclePlateAlreadyExistsAsync(request.NewPlate);
            
            if (motorcyclePlateAlreadyExist)
            {
                _logger.LogError($"Interrompendo processo de atualização de placa. Motivo: Placa da moto duplicada. Placa: {request.NewPlate}");
                throw new MotorcycleAlreadyExistsException(request.NewPlate);
            }

            motorcycleById.ChangePlate(request.NewPlate);
            await _motorcycleRepository.SaveChangesAsync();

            _logger.LogTrace($"Finalizando processo de atualização de placa. Id do registro atualizado: {motorcycleById.Id}");

            return Unit.Value;
        }
    }
}