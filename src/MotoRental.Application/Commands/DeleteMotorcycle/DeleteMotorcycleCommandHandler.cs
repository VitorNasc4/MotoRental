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

        public DeleteMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }
        public async Task<Unit> Handle(DeleteMotorcycleCommand request, CancellationToken cancellationToken)
        {
            var motorcycleById = await _motorcycleRepository.GetMotorcycleByIdAsync(request.MotorcycleId);
            
            if (motorcycleById is null)
            {
                throw new MotorcycleNotFoundException(request.MotorcycleId);
            }

            await _motorcycleRepository.RemoveAsync(motorcycleById);

            return Unit.Value;
        }
    }
}