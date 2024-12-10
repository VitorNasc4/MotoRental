using System.Threading;
using System.Threading.Tasks;
using MotoRental.Core.Repositories;
using MediatR;
using MotoRental.Core.Exceptions;

namespace MotoRental.Application.Commands.CreateRental
{
    public class CreateRentalCommandHandler : IRequestHandler<CreateRentalCommand, Unit>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;

        public CreateRentalCommandHandler(IRentalRepository rentalRepository, IMotorcycleRepository motorcycleRepository, IDeliveryPersonRepository deliveryPersonRepository)
        {
            _rentalRepository = rentalRepository;
            _motorcycleRepository = motorcycleRepository;
            _deliveryPersonRepository = deliveryPersonRepository;

        }
        public async Task<Unit> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
        {
            var motorcycle = await _motorcycleRepository.GetMotorcycleByIdAsync(request.moto_id);
            if (motorcycle is null)
            {
                throw new MotorcycleNotFoundException(request.moto_id);
            }

            var deliveryPerson = await _deliveryPersonRepository.GetDeliveryPersonByIdAsync(request.entregador_id);
            if (deliveryPerson is null)
            {
                throw new DeliveryPersonNotFoundException(request.entregador_id);
            }

            var rental = CreateRentalCommand.ToEntity(request);

            await _rentalRepository.AddAsync(rental);

            return Unit.Value;
        }
    }
}