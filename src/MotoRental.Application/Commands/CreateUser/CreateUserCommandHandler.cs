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

namespace MotoRental.Application.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        public CreateUserCommandHandler(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }
        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var emailAlreadyExist = await _userRepository.CheckEmailExist(request.Email);

            if (emailAlreadyExist)
            {
                throw new EmailAlreadyExistsException(request.Email);
            }

            var passwordHash = _authService.ComputeSha256Hash(request.Password);
            var user = CreateUserCommand.ToEntity(request, passwordHash);

            await _userRepository.AddAsync(user);

            return user.Id;
        }
    }
}