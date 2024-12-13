using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MotoRental.Application.ViewModels;
using MotoRental.Core.Repositories;
using MotoRental.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MotoRental.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace MotoRental.Application.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserViewModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GetUserQueryHandler> _logger;

        public GetUserQueryHandler(IUserRepository userRepository, ILogger<GetUserQueryHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }
        public async Task<UserViewModel> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Iniciando processo de busca de usuário pelo Id: {request.Id}");
            var user = await _userRepository.GetUserByIdAsync(request.Id);

            if (user is null)
            {
                _logger.LogError($"Interrompendo processo de busca. Motivo: Registro de usuário não encontrado. Id buscado: {request.Id}");
                throw new UserIdNotFoundException(request.Id);
            }

            _logger.LogInformation($"Finalizando processo de busca de usuário pelo Id: {request.Id}. Registro encontrado com sucesso");
            return new UserViewModel(user.FullName, user.Email);
        }
    }
}