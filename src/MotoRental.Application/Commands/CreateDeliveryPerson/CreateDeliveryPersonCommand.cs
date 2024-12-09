using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MotoRental.Core.Entities;

namespace MotoRental.Application.Commands.CreateDeliveryPerson
{
    public class CreateDeliveryPersonCommand : IRequest<Unit>
    {
        public string identificador { get; set; }
        public string nome { get; set; }
        public string cnpj { get; set; }
        public DateTime data_nascimento { get; set; }
        public string numero_cnh { get; set; }
        public string tipo_cnh { get; set; }
        public string imagem_cnh { get; set; }

        public static DeliveryPerson ToEntity(CreateDeliveryPersonCommand command)
        {
            return new DeliveryPerson(command.identificador, command.nome, command.cnpj, command.data_nascimento, command.numero_cnh, command.tipo_cnh.ToUpper(), command.imagem_cnh = null);
        }

    }
}