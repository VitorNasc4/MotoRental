using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.Core.Exceptions
{
    public class MotorcycleRentalHistoricFoundException : Exception
    {
        public MotorcycleRentalHistoricFoundException(string id)
            : base($"Existe um histórico de aluguel com id '{id}' referente a essa moto")
        {
        }
    }
}