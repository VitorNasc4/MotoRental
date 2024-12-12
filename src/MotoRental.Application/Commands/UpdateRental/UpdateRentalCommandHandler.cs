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

namespace MotoRental.Application.Commands.UpdateRental
{
    public class UpdateRentalCommandHandler : IRequestHandler<UpdateRentalCommand>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly ILogger<UpdateRentalCommandHandler> _logger;

        public UpdateRentalCommandHandler(IRentalRepository rentalRepository, ILogger<UpdateRentalCommandHandler> logger)
        {
            _rentalRepository = rentalRepository;
            _logger = logger;
        }
        public async Task<Unit> Handle(UpdateRentalCommand request, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"Iniciando processo de atualização de aluguel. Id do aluguel: {request.RentalId}");

            var rentalById = await _rentalRepository.GetRentalByIdAsync(request.RentalId);

            if (rentalById is null)
            {
                _logger.LogError($"Interrompendo processo de atualização de aluguel. Motivo: Registro de aluguel não encontrado. Id buscado: {request.RentalId}");
                throw new RentalNotFoundException(request.RentalId);
            }
            
            rentalById.SetReturnDate(request.NewDate);
            await _rentalRepository.SaveChangesAsync();

            _logger.LogTrace($"Finalizando processo de atualização de aluguel. Id do registro atualizado: {rentalById.Id}");

            return Unit.Value;
        }
    }
}