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

namespace MotoRental.Application.Commands.UpdateMotorcycle
{
    public class UpdateMotorcycleCommandHandler : IRequestHandler<UpdateMotorcycleCommand>
    {
        private readonly IMotorcycleRepository _motorcycleRepository;

        public UpdateMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }
        public async Task<Unit> Handle(UpdateMotorcycleCommand request, CancellationToken cancellationToken)
        {
            var motorcycleById = await _motorcycleRepository.GetMotorcycleByIdAsync(request.MotorcycleId);

            if (motorcycleById is null)
            {
                throw new MotorcycleNotFoundException(request.MotorcycleId);
            }

            var motorcyclePlateAlreadyExist = await _motorcycleRepository.MotorcyclePlateAlreadyExistsAsync(request.NewPlate);
            
            if (motorcyclePlateAlreadyExist)
            {
                throw new MotorcycleAlreadyExistsException(request.NewPlate);
            }

            motorcycleById.ChangePlate(request.NewPlate);
            await _motorcycleRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}