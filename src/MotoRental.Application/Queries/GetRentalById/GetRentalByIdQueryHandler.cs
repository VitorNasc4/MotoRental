using System.Threading;
using System.Threading.Tasks;
using MotoRental.Application.ViewModels;
using MotoRental.Core.Repositories;
using MediatR;
using MotoRental.Core.Exceptions;

namespace MotoRental.Application.Queries.GetRentalById
{
    public class GetRentalByIdQueryHandler : IRequestHandler<GetRentalByIdQuery, RentalViewModel>
    {
        private readonly IRentalRepository _rentalRepository;

        public GetRentalByIdQueryHandler(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }
        public async Task<RentalViewModel> Handle(GetRentalByIdQuery request, CancellationToken cancellationToken)
        {
            var rental = await _rentalRepository.GetRentalByIdAsync(request.Id);

            if (rental == null)
            {
                throw new RentalNotFoundException(request.Id);
            }

            return new RentalViewModel(
                rental.Id, 
                rental.PlanDailyRate, 
                rental.DeliveryPersonId, 
                rental.MotorcycleId,
                rental.StartDate,
                rental.EndDate,
                rental.ExpectedReturnDate,
                rental.ActualReturnDate
            );
        }
    }
}