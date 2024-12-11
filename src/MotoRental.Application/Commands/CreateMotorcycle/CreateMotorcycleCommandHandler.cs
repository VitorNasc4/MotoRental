using System.Threading;
using System.Threading.Tasks;
using MotoRental.Core.Repositories;
using MotoRental.Core.Services;
using MediatR;
using MotoRental.Core.Exceptions;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace MotoRental.Application.Commands.CreateMotorcycle
{
    public class CreateMotorcycleCommandHandler : IRequestHandler<CreateMotorcycleCommand, Unit>
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IMessageBusService _messageBusService;
        private readonly IConfiguration _configuration;

        public CreateMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository, IMessageBusService messageBusService, IConfiguration configuration)
        {
            _motorcycleRepository = motorcycleRepository;
            _messageBusService = messageBusService;
            _configuration = configuration;
        }
        public async Task<Unit> Handle(CreateMotorcycleCommand request, CancellationToken cancellationToken)
        {
            var motorcyclePlateAlreadyExist = await _motorcycleRepository.MotorcyclePlateAlreadyExistsAsync(request.placa);
            
            if (motorcyclePlateAlreadyExist)
            {
                throw new MotorcycleAlreadyExistsException(request.placa);
            }

            var motorcycleDTO = CreateMotorcycleCommand.ToDTO(request);

            var motorcycleInfoJson = JsonSerializer.Serialize(motorcycleDTO);
            var motorcycleInfoBytes = Encoding.UTF8.GetBytes(motorcycleInfoJson);

            var queueName = _configuration["RabbitmqConfig:QueueName"];

            _messageBusService.Publish(queueName, motorcycleInfoBytes);

            return Unit.Value;
        }
    }
}