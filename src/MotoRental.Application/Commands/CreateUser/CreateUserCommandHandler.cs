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
using Microsoft.Extensions.Logging;

namespace MotoRental.Application.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly ILogger<CreateUserCommandHandler> _logger;
        public CreateUserCommandHandler(
            IUserRepository userRepository, 
            IAuthService authService,
            ILogger<CreateUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _authService = authService;
            _logger = logger;
        }
        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"Iniciando processo de registro de novo usuário. Dados: {request}");

            var emailAlreadyExist = await _userRepository.CheckEmailExist(request.Email);

            if (emailAlreadyExist)
            {
                _logger.LogError($"Interrompendo processo de registro de novo usuário. Motivo: Email {request.Email} já existente ");
                throw new EmailAlreadyExistsException(request.Email);
            }

            var passwordHash = _authService.ComputeSha256Hash(request.Password);
            var user = CreateUserCommand.ToEntity(request, passwordHash);

            await _userRepository.AddAsync(user);

            _logger.LogTrace($"Finalizando processo de registro de novo usuário com sucesso. Id de registro: {user.Id}");

            return user.Id;
        }
    }
}