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

namespace MotoRental.Application.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly INotificationService _notificationService;

        public CreateUserCommandHandler(IUserRepository userRepository, IAuthService authService, INotificationService notificationService)
        {
            _userRepository = userRepository;
            _authService = authService;
            _notificationService = notificationService;
        }
        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var passwordHash = _authService.ComputeSha256Hash(request.Password);
            var user = CreateUserCommand.ToEntity(request, passwordHash);

            await _userRepository.AddAsync(user);

            var notificationInfoDTO = User.ToDTO(user);

            _notificationService.ProcessNotification(notificationInfoDTO);

            return user.Id;
        }
    }
}