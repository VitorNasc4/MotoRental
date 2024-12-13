using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.Core.Exceptions
{
    public class MotorcycleNotFoundException : Exception
    {
        public MotorcycleNotFoundException(string id)
            : base($"Moto com o id '{id}' não foi encontrada")
        {
        }
    }
}