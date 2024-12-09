using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MotoRental.Core.Entities;

namespace MotoRental.Application.Commands.DeleteMotorcycle
{
    public class DeleteMotorcycleCommand : IRequest<Unit>
    {
        public DeleteMotorcycleCommand(int motorcycleId)
        {
            MotorcycleId = motorcycleId;
        }
        public int MotorcycleId { get; set; }

    }
}