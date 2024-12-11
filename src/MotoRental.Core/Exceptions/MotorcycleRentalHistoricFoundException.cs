using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.Core.Exceptions
{
    public class MotorcycleRentalHistoricFoundException : Exception
    {
        public MotorcycleRentalHistoricFoundException(string id)
            : base($"A motorcycle rental exist with id '{id}'.")
        {
        }
    }
}