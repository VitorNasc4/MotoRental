using System.Threading;
using System.Threading.Tasks;
using MotoRental.Core.Entities;
using MotoRental.Core.Repositories;
using MotoRental.Core.Services;
using MediatR;
using MotoRental.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace MotoRental.Application.Commands.UpdateUserToAdmin
{
    public class UpdateUserToAdminCommandHandler : IRequestHandler<UpdateUserToAdminCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UpdateUserToAdminCommandHandler> _logger;

        public UpdateUserToAdminCommandHandler(IUserRepository userRepository, ILogger<UpdateUserToAdminCommandHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }
        public async Task<Unit> Handle(UpdateUserToAdminCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Iniciando processo de atualização de registro de usuário. Id do usuário: {request.UserId}, IsAdmin: {request.IsAdmin}");

            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            
            if (user is null)
            {
                _logger.LogError($"Interrompendo processo de atualização de registro de usuário. Motivo: Usuário não encontrado. Id buscado: {request.UserId}");
                throw new UserIdNotFoundException(request.UserId);
            }

            user.SetAdmin(request.IsAdmin);
            
            await _userRepository.SaveChangesAsync();

            _logger.LogInformation($"Finalizando processo de atualização de registro de usuário. Id do registro atualizado: {user.Id}");

            return Unit.Value;
        }
    }
}