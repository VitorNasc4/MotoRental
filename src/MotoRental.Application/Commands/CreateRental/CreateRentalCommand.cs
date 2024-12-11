using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MotoRental.Core.Entities;
using MotoRental.Core.Exceptions;

namespace MotoRental.Application.Commands.CreateRental
{
    public class CreateRentalCommand : IRequest<Unit>
    {
        public string entregador_id { get; set; }
        public string moto_id { get; set; }
        public DateTime data_inicio { get; set; }
        public DateTime data_termino { get; set; }
        public DateTime data_previsao_termino { get; set; }
        public int plano { get; set; }


        public static Rental ToEntity(CreateRentalCommand command)
        {
            if (!Rental.IsValidPlan(command.plano))
            {
                throw new InvalidRentalException(command.plano);
            }

            return new Rental(command.moto_id, command.entregador_id, command.plano, command.data_inicio, command.data_termino, command.data_previsao_termino);
        }

    }
}