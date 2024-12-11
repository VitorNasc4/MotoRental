using System.Threading;
using System.Threading.Tasks;
using MotoRental.Core.Repositories;
using MediatR;
using MotoRental.Core.Exceptions;

namespace MotoRental.Application.Commands.CreateDeliveryPerson
{
    public class CreateDeliveryPersonCommandHandler : IRequestHandler<CreateDeliveryPersonCommand, Unit>
    {
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;

        public CreateDeliveryPersonCommandHandler(IDeliveryPersonRepository deliveryPersonRepository)
        {
            _deliveryPersonRepository = deliveryPersonRepository;

        }
        public async Task<Unit> Handle(CreateDeliveryPersonCommand request, CancellationToken cancellationToken)
        {
            var deliveryPersonAlreadyExist = await _deliveryPersonRepository.DeliveryPersonAlreadyExistsAsync(request.numero_cnh, request.cnpj);
            
            if (deliveryPersonAlreadyExist)
            {
                throw new DeliveryPersonAlreadyExistsException(request.numero_cnh, request.cnpj);
            }

            var deliveryPerson = CreateDeliveryPersonCommand.ToEntity(request);

            await _deliveryPersonRepository.AddAsync(deliveryPerson);

            return Unit.Value;
        }
    }
}