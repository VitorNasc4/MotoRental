using System.Threading;
using System.Threading.Tasks;
using MotoRental.Core.Repositories;
using MotoRental.Core.Services;
using MediatR;
using MotoRental.Core.Exceptions;
using System.Text.Json;
using System.Text;

namespace MotoRental.Application.Commands.CreateMotorcycle
{
    public class CreateMotorcycleCommandHandler : IRequestHandler<CreateMotorcycleCommand, Unit>
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IMessageBusService _messageBusService;
        private const string QUEUE_NAME = "motorcycle-events";

        public CreateMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository, IMessageBusService messageBusService)
        {
            _motorcycleRepository = motorcycleRepository;
            _messageBusService = messageBusService;
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

            _messageBusService.Publish(QUEUE_NAME, motorcycleInfoBytes);

            return Unit.Value;
        }
    }
}