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
        public GetRentalByIdQuery(int id)
        {
            Id = id;
        }
        public int Id { get; private set; }
    }
}