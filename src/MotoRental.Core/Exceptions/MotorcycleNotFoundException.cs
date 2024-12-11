using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.Core.Exceptions
{
    public class MotorcycleNotFoundException : Exception
    {
        public MotorcycleNotFoundException(string id)
            : base($"A motorcycle with id '{id}' not found.")
        {
        }
    }
}