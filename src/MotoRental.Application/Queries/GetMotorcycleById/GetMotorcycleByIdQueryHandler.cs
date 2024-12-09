using System.Threading;
using System.Threading.Tasks;
using MotoRental.Application.ViewModels;
using MotoRental.Core.Repositories;
using MediatR;
using MotoRental.Core.Exceptions;

namespace MotoRental.Application.Queries.GetMotorcycleById
{
    public class GetMotorcycleByIdQueryHandler : IRequestHandler<GetMotorcycleByIdQuery, MotorcylceViewModel>
    {
        private readonly IMotorcycleRepository _motorcycleRepository;

        public GetMotorcycleByIdQueryHandler(IMotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }
        public async Task<MotorcylceViewModel> Handle(GetMotorcycleByIdQuery request, CancellationToken cancellationToken)
        {
            var motorcycle = await _motorcycleRepository.GetMotorcycleByIdAsync(request.Id);

            if (motorcycle == null)
            {
                throw new MotorcycleNotFoundException(request.Id);
            }

            return new MotorcylceViewModel(motorcycle.Identifier, motorcycle.Year, motorcycle.Model, motorcycle.Plate);
        }
    }
}