using System.Threading;
using System.Threading.Tasks;
using MotoRental.Core.Entities;
using MotoRental.Core.Repositories;
using MotoRental.Core.Services;
using MediatR;

namespace MotoRental.Application.Commands.CreateUser
{
    public class CreateUserAdminCommandHandler : IRequestHandler<CreateUserAdminCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public CreateUserAdminCommandHandler(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }
        public async Task<string> Handle(CreateUserAdminCommand request, CancellationToken cancellationToken)
        {
            var passwordHash = _authService.ComputeSha256Hash(request.Password);
            var user = CreateUserAdminCommand.ToEntity(request, passwordHash);

            await _userRepository.AddAsync(user);
            return user.Id;
        }
    }
}