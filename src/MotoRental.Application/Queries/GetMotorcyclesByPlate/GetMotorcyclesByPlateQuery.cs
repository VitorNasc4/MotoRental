using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotoRental.Application.ViewModels;
using MediatR;

namespace MotoRental.Application.Queries.GetMotorcycleByPlate
{
    public class GetMotorcyclesByPlateQuery : IRequest<List<MotorcylceViewModel>>
    {
        public GetMotorcyclesByPlateQuery(string plate)
        {
            Plate = plate;
        }
        public string Plate { get; private set; }
    }
}