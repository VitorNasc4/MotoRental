using System.Threading;
using System.Threading.Tasks;
using MotoRental.Application.ViewModels;
using MotoRental.Core.Repositories;
using MediatR;
using MotoRental.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace MotoRental.Application.Queries.GetMotorcycleById
{
    public class GetMotorcycleByIdQueryHandler : IRequestHandler<GetMotorcycleByIdQuery, MotorcylceViewModel>
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly ILogger<GetMotorcycleByIdQueryHandler> _logger;

        public GetMotorcycleByIdQueryHandler(IMotorcycleRepository motorcycleRepository, ILogger<GetMotorcycleByIdQueryHandler> logger)
        {
            _motorcycleRepository = motorcycleRepository;
            _logger = logger;
        }
        public async Task<MotorcylceViewModel> Handle(GetMotorcycleByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"Iniciando processo de busca de moto pela Id: {request.Id}");
            var motorcycle = await _motorcycleRepository.GetMotorcycleByIdAsync(request.Id);

            if (motorcycle == null)
            {
                _logger.LogError($"Interrompendo processo de busca. Motivo: Registro de moto não encontrado. Id buscado: {request.Id}");
                throw new MotorcycleNotFoundException(request.Id);
            }

            _logger.LogTrace($"Finalizando processo de busca de moto pela Id: {request.Id}. Registro encontrado com sucesso");

            return new MotorcylceViewModel(motorcycle.Id, motorcycle.Year, motorcycle.Model, motorcycle.Plate);
        }
    }
}