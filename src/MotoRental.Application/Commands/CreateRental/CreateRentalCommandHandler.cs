using System.Threading;
using System.Threading.Tasks;
using MotoRental.Core.Repositories;
using MediatR;
using MotoRental.Core.Exceptions;
using MotoRental.Core.Entities;
using Microsoft.Extensions.Logging;

namespace MotoRental.Application.Commands.CreateRental
{
    public class CreateRentalCommandHandler : IRequestHandler<CreateRentalCommand, Unit>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;
        private readonly ILogger<CreateRentalCommandHandler> _logger;

        public CreateRentalCommandHandler(
            IRentalRepository rentalRepository, 
            IMotorcycleRepository motorcycleRepository, 
            IDeliveryPersonRepository deliveryPersonRepository,
            ILogger<CreateRentalCommandHandler> logger)
        {
            _rentalRepository = rentalRepository;
            _motorcycleRepository = motorcycleRepository;
            _deliveryPersonRepository = deliveryPersonRepository;
            _logger = logger;
        }
        public async Task<Unit> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Iniciando processo de registro de novo aluguel. Dados: {request}");

            var motorcycle = await _motorcycleRepository.GetMotorcycleByIdAsync(request.moto_id);
            if (motorcycle is null)
            {
                _logger.LogError($"Interrompendo processo de registro de novo aluguel. Motivo: Id da moto {request.moto_id} não encontrado");
                throw new MotorcycleNotFoundException(request.moto_id);
            }

            var deliveryPerson = await _deliveryPersonRepository.GetDeliveryPersonByIdAsync(request.entregador_id);
            if (deliveryPerson is null)
            {            
                _logger.LogError($"Interrompendo processo de registro de novo aluguel. Motivo: Id do entregador {request.entregador_id} não encontrado");
                throw new DeliveryPersonNotFoundException(request.entregador_id);
            }

            if (deliveryPerson.CNH_Type != CNH_Types.Type_A && deliveryPerson.CNH_Type != CNH_Types.Type_AB)
            {
                _logger.LogError($"Interrompendo processo de registro de novo aluguel. Motivo: O entregador não possui a permissão necessária. Permissão do entregador: {deliveryPerson.CNH_Type}");
                throw new DeliveryPersonCnhTypeException(request.entregador_id);
            }

            var rental = CreateRentalCommand.ToEntity(request);

            await _rentalRepository.AddAsync(rental);

            _logger.LogInformation($"Finalizando processo de registro de novo aluguel com sucesso. Id de registro: {rental.Id}");

            return Unit.Value;
        }
    }
}