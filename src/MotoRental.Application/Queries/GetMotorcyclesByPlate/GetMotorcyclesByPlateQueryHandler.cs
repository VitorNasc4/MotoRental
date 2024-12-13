using System.Threading;
using System.Threading.Tasks;
using MotoRental.Application.ViewModels;
using MotoRental.Core.Repositories;
using MediatR;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace MotoRental.Application.Queries.GetMotorcycleByPlate
{
    public class GetMotorcyclesByPlateQueryHandler : IRequestHandler<GetMotorcyclesByPlateQuery, List<MotorcylceViewModel>>
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly ILogger<GetMotorcyclesByPlateQueryHandler> _logger;

        public GetMotorcyclesByPlateQueryHandler(IMotorcycleRepository motorcycleRepository, ILogger<GetMotorcyclesByPlateQueryHandler> logger)
        {
            _motorcycleRepository = motorcycleRepository;
            _logger = logger;
        }
        public async Task<List<MotorcylceViewModel>> Handle(GetMotorcyclesByPlateQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Iniciando processo de busca de moto pela placa: {request.Plate}");
            var motorcycles = await _motorcycleRepository.GetMotorcyclesByPlateAsync(request.Plate);
            var motorcyclesViewModelList = new List<MotorcylceViewModel>();

            if (motorcycles.Count == 0)
            {
                _logger.LogInformation($"Finalizando com sucesso processo de busca de moto por placa. Não foi encontrado nenhum resultado");
                return motorcyclesViewModelList;
            }

            foreach (var motorcycle in motorcycles)
            {
                motorcyclesViewModelList.Add(new MotorcylceViewModel(motorcycle.Id, motorcycle.Year, motorcycle.Model, motorcycle.Plate));
            }

            _logger.LogInformation($"Finalizando com sucesso processo de busca de moto por placa. Foram encontrados {motorcyclesViewModelList.Count} resultados");

            return motorcyclesViewModelList;
        }
    }
}