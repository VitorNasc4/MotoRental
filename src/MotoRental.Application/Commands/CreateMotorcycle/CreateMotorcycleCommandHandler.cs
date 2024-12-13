using System.Threading;
using System.Threading.Tasks;
using MotoRental.Core.Repositories;
using MotoRental.Core.Services;
using MediatR;
using MotoRental.Core.Exceptions;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MotoRental.Application.Commands.CreateMotorcycle
{
    public class CreateMotorcycleCommandHandler : IRequestHandler<CreateMotorcycleCommand, Unit>
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IMessageBusService _messageBusService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CreateMotorcycleCommandHandler> _logger;

        public CreateMotorcycleCommandHandler(
            IMotorcycleRepository motorcycleRepository, 
            IMessageBusService messageBusService, 
            IConfiguration configuration,
            ILogger<CreateMotorcycleCommandHandler> logger)
        {
            _motorcycleRepository = motorcycleRepository;
            _messageBusService = messageBusService;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<Unit> Handle(CreateMotorcycleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Iniciando processo de registro de nova moto. Dados: {request}");

            var motorcyclePlateAlreadyExist = await _motorcycleRepository.MotorcyclePlateAlreadyExistsAsync(request.placa);
            
            if (motorcyclePlateAlreadyExist)
            {
                _logger.LogError($"Interrompendo processo de registro de nova moto. Motivo: Placa da moto duplicada. Placa: {request.placa}");
                throw new MotorcycleAlreadyExistsException(request.placa);
            }

            var motorcycleDTO = CreateMotorcycleCommand.ToDTO(request);

            var motorcycleInfoJson = JsonSerializer.Serialize(motorcycleDTO);
            var motorcycleInfoBytes = Encoding.UTF8.GetBytes(motorcycleInfoJson);

            var queueName = _configuration["RabbitmqConfig:QueueName"];

            _logger.LogInformation($"Publicando evento de registro de nova moto");

            _messageBusService.Publish(queueName, motorcycleInfoBytes);

            _logger.LogInformation($"Evento de registro de nova moto publicado com sucesso");

            return Unit.Value;
        }
    }
}