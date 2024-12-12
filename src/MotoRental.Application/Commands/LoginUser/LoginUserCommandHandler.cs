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

namespace MotoRental.Application.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserViewModel>
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        public LoginUserCommandHandler(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }
        public async Task<LoginUserViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var passwordHash = _authService.ComputeSha256Hash(request.Password);

            var user = await _userRepository.GetUserByEmailAndPasswordAsyn(request.Email, passwordHash);

            if (user is null)
            {
                throw new UserNotFoundException();
            }

            var token = _authService.GenerateJWTToken(user.Email, user.Role);
            var loginUserViewModel = new LoginUserViewModel(user.Email, token);

            return loginUserViewModel;
        }
    }
}