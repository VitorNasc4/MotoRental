using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MotoRental.Application.ViewModels;
using MotoRental.Core.Repositories;
using MotoRental.Core.Services;
using MediatR;
using MotoRental.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace MotoRental.Application.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserViewModel>
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<LoginUserCommandHandler> _logger;
        public LoginUserCommandHandler(
            IAuthService authService, 
            IUserRepository userRepository,
            ILogger<LoginUserCommandHandler> logger)
        {
            _authService = authService;
            _userRepository = userRepository;
            _logger = logger;
        }
        public async Task<LoginUserViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Iniciando processo de login. Email do usuário: {request.Email}");

            var passwordHash = _authService.ComputeSha256Hash(request.Password);

            var user = await _userRepository.GetUserByEmailAndPasswordAsyn(request.Email, passwordHash);

            if (user is null)
            {
                _logger.LogError($"Interrompendo processo de login. Motivo: Email ou senha incorreto");
                throw new UserNotFoundException();
            }

            var token = _authService.GenerateJWTToken(user.Email, user.Role);
            var loginUserViewModel = new LoginUserViewModel(user.Email, token);

            _logger.LogInformation($"Finalizando processo de login. Token gerado com sucesso");

            return loginUserViewModel;
        }
    }
}