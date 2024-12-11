using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotoRental.Application.ViewModels;
using MediatR;

namespace MotoRental.Application.Queries.GetMotorcycleById
{
    public class GetMotorcycleByIdQuery : IRequest<MotorcylceViewModel>
    {
        public GetMotorcycleByIdQuery(string id)
        {
            Id = id;
        }
        public string Id { get; private set; }
    }
}