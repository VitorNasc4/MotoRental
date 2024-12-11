using System.Threading;
using System.Threading.Tasks;
using MotoRental.Application.ViewModels;
using MotoRental.Core.Repositories;
using MediatR;
using System.Collections.Generic;

namespace MotoRental.Application.Queries.GetMotorcycleByPlate
{
    public class GetMotorcyclesByPlateQueryHandler : IRequestHandler<GetMotorcyclesByPlateQuery, List<MotorcylceViewModel>>
    {
        private readonly IMotorcycleRepository _motorcycleRepository;

        public GetMotorcyclesByPlateQueryHandler(IMotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }
        public async Task<List<MotorcylceViewModel>> Handle(GetMotorcyclesByPlateQuery request, CancellationToken cancellationToken)
        {
            var motorcycles = await _motorcycleRepository.GetMotorcyclesByPlateAsync(request.Plate);
            var motorcyclesViewModelList = new List<MotorcylceViewModel>();

            if (motorcycles.Count == 0)
            {
                return motorcyclesViewModelList;
            }

            foreach (var motorcycle in motorcycles)
            {
                motorcyclesViewModelList.Add(new MotorcylceViewModel(motorcycle.Id, motorcycle.Year, motorcycle.Model, motorcycle.Plate));
            }

            return motorcyclesViewModelList;
        }
    }
}