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

namespace MotoRental.Application.Commands.CreateMotorcycle
{
    public class CreateMotorcycleCommandHandler : IRequestHandler<CreateMotorcycleCommand, int>
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly INotificationService _notificationService;

        public CreateMotorcycleCommandHandler(IMotorcycleRepository motorcycleRepository, INotificationService notificationService)
        {
            _motorcycleRepository = motorcycleRepository;
            _notificationService = notificationService;
        }
        public async Task<int> Handle(CreateMotorcycleCommand request, CancellationToken cancellationToken)
        {
            var motorcyclePlateAlreadyExist = await _motorcycleRepository.MotorcyclePlateAlreadyExistsAsync(request.placa);
            
            if (motorcyclePlateAlreadyExist)
            {
                throw new MotorcycleAlreadyExistsException(request.placa);
            }

            var motorcycle = CreateMotorcycleCommand.ToEntity(request);

            await _motorcycleRepository.AddAsync(motorcycle);

            // var notificationInfoDTO = Motorcycle.ToDTO(motorcycle);

            // _notificationService.ProcessNotification(notificationInfoDTO);

            return motorcycle.Id;
        }
    }
}