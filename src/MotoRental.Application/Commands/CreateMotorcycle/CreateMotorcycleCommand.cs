using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MotoRental.Core.Entities;

namespace MotoRental.Application.Commands.CreateMotorcycle
{
    public class CreateMotorcycleCommand : IRequest<int>
    {
        public string identificador { get; set; }
        public string ano { get; set; }
        public string modelo { get; set; }
        public string placa { get; set; }

        public static Motorcycle ToEntity(CreateMotorcycleCommand command)
        {
            return new Motorcycle(command.identificador, command.ano, command.modelo, command.placa);
        }

    }
}