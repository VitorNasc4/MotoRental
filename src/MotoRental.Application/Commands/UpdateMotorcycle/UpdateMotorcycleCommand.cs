using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MotoRental.Core.Entities;

namespace MotoRental.Application.Commands.UpdateMotorcycle
{
    public class UpdateMotorcycleCommand : IRequest
    {
        public UpdateMotorcycleCommand(string motorcycleId, string newPlate)
        {
            MotorcycleId = motorcycleId;
            NewPlate = newPlate;
        }
        
        public string MotorcycleId { get; set; }
        public string NewPlate { get; set; }
    }
}