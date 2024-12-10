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

namespace MotoRental.Application.Commands.DeleteMotorcycle
{
    public class DeleteMotorcycleCommandHandler : IRequestHandler<DeleteMotorcycleCommand, Unit>
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IRentalRepository _rentalRepository;

        public DeleteMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository, IRentalRepository rentalRepository)
        {
            _motorcycleRepository = motorcycleRepository;
            _rentalRepository = rentalRepository;
        }
        public async Task<Unit> Handle(DeleteMotorcycleCommand request, CancellationToken cancellationToken)
        {
            var motorcycleById = await _motorcycleRepository.GetMotorcycleByIdAsync(request.MotorcycleId);
            
            if (motorcycleById is null)
            {
                throw new MotorcycleNotFoundException(request.MotorcycleId);
            }

            var motorcycleRentalExists = await _rentalRepository.CheckMotorcycleRentalHistoricAsync(request.MotorcycleId);

            if (motorcycleRentalExists)
            {
                throw new MotorcycleRentalHistoricFoundException(request.MotorcycleId);
            }

            await _motorcycleRepository.RemoveAsync(motorcycleById);

            return Unit.Value;
        }
    }
}