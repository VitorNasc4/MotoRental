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

namespace MotoRental.Application.Commands.DeleteMotorcycle
{
    public class DeleteMotorcycleCommandHandler : IRequestHandler<DeleteMotorcycleCommand, Unit>
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly ILogger<DeleteMotorcycleCommandHandler> _logger;

        public DeleteMotorcycleCommandHandler(
            IMotorcycleRepository motorcycleRepository, 
            IRentalRepository rentalRepository,
            ILogger<DeleteMotorcycleCommandHandler> logger)
        {
            _motorcycleRepository = motorcycleRepository;
            _rentalRepository = rentalRepository;
            _logger = logger;
        }
        public async Task<Unit> Handle(DeleteMotorcycleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"Iniciando processo de deleção de moto. Id da moto a ser deletada: {request.MotorcycleId}");

            var motorcycleById = await _motorcycleRepository.GetMotorcycleByIdAsync(request.MotorcycleId);
            
            if (motorcycleById is null)
            {
                
                _logger.LogError($"Interrompendo processo de deleção de moto. Motivo: Não foi encontrado registro de moto com id {request.MotorcycleId}");
                throw new MotorcycleNotFoundException(request.MotorcycleId);
            }

            var motorcycleRentalExists = await _rentalRepository.CheckMotorcycleRentalHistoricAsync(request.MotorcycleId);

            if (motorcycleRentalExists)
            {
                _logger.LogError($"Interrompendo processo de deleção de moto. Motivo: A moto com id {request.MotorcycleId} possui histórico de locação");
                throw new MotorcycleRentalHistoricFoundException(request.MotorcycleId);
            }

            await _motorcycleRepository.RemoveAsync(motorcycleById);

            _logger.LogTrace($"Finalizando processo de deleção de moto com sucesso.");

            return Unit.Value;
        }
    }
}