using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.Core.Exceptions
{
    public class RentalNotFoundException : Exception
    {
        public RentalNotFoundException(string id)
            : base($"Aluguel com '{id}' não foi encontrado")
        {
        }
    }
}