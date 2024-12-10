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

namespace MotoRental.Application.Commands.UpdateRental
{
    public class UpdateRentalCommandHandler : IRequestHandler<UpdateRentalCommand>
    {
        private readonly IRentalRepository _rentalRepository;

        public UpdateRentalCommandHandler(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }
        public async Task<Unit> Handle(UpdateRentalCommand request, CancellationToken cancellationToken)
        {
            var rentalById = await _rentalRepository.GetRentalByIdAsync(request.RentalId);

            if (rentalById is null)
            {
                throw new RentalNotFoundException(request.RentalId);
            }
            
            rentalById.SetReturnDate(request.NewDate);
            await _rentalRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}