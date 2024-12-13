using System.Threading;
using System.Threading.Tasks;
using MotoRental.Application.ViewModels;
using MotoRental.Core.Repositories;
using MediatR;
using MotoRental.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace MotoRental.Application.Queries.GetRentalById
{
    public class GetRentalByIdQueryHandler : IRequestHandler<GetRentalByIdQuery, RentalViewModel>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly ILogger<GetRentalByIdQueryHandler> _logger;

        public GetRentalByIdQueryHandler(IRentalRepository rentalRepository, ILogger<GetRentalByIdQueryHandler> logger)
        {
            _rentalRepository = rentalRepository;
            _logger = logger;
        }
        public async Task<RentalViewModel> Handle(GetRentalByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Iniciando processo de busca de aluguel pelo Id: {request.Id}");
            var rental = await _rentalRepository.GetRentalByIdAsync(request.Id);

            if (rental == null)
            {
                _logger.LogError($"Interrompendo processo de aluguel. Motivo: Registro de aluguel não encontrado. Id buscado: {request.Id}");
                throw new RentalNotFoundException(request.Id);
            }

            _logger.LogInformation($"Finalizando processo de busca de aluguel pelo Id: {request.Id}. Registro encontrado com sucesso");

            return new RentalViewModel(
                rental.Id, 
                rental.PlanDailyRate, 
                rental.DeliveryPersonId, 
                rental.MotorcycleId,
                rental.StartDate,
                rental.EndDate,
                rental.ExpectedReturnDate,
                rental.ActualReturnDate,
                rental.TotalCost,
                rental.Penalty
            );
        }
    }
}