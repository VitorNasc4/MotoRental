using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotoRental.Application.ViewModels;
using MediatR;

namespace MotoRental.Application.Queries.GetRentalById
{
    public class GetRentalByIdQuery : IRequest<RentalViewModel>
    {
        public GetRentalByIdQuery(string id)
        {
            Id = id;
        }
        public string Id { get; private set; }
    }
}