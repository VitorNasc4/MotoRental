using System.Threading;
using System.Threading.Tasks;
using MotoRental.Core.Repositories;
using MediatR;
using MotoRental.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace MotoRental.Application.Commands.CreateDeliveryPerson
{
    public class CreateDeliveryPersonCommandHandler : IRequestHandler<CreateDeliveryPersonCommand, Unit>
    {
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;
        private readonly ILogger<CreateDeliveryPersonCommandHandler> _logger;

        public CreateDeliveryPersonCommandHandler(IDeliveryPersonRepository deliveryPersonRepository, ILogger<CreateDeliveryPersonCommandHandler> logger)
        {
            _deliveryPersonRepository = deliveryPersonRepository;
            _logger = logger;

        }
        public async Task<Unit> Handle(CreateDeliveryPersonCommand request, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"Iniciando processo de registro de novo entregador. Dados: {request}");

            var deliveryPersonAlreadyExist = await _deliveryPersonRepository.DeliveryPersonAlreadyExistsAsync(request.numero_cnh, request.cnpj);
            
            if (deliveryPersonAlreadyExist)
            {
                _logger.LogError($"Interrompendo processo de registro de novo entregador. Motivo: Dados de CNH ou CNPJ duplicados. CNH: {request.numero_cnh} cnpj: {request.cnpj}");
                throw new DeliveryPersonAlreadyExistsException(request.numero_cnh, request.cnpj);
            }

            var deliveryPerson = CreateDeliveryPersonCommand.ToEntity(request);

            await _deliveryPersonRepository.AddAsync(deliveryPerson);

            _logger.LogTrace($"Finalizando processo de registro de novo entregador com sucesso. Id de registro: {deliveryPerson.Id}");

            return Unit.Value;
        }
    }
}