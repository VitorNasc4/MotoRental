using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.Core.Exceptions
{
    public class MotorcycleAlreadyExistsException : Exception
    {
        public MotorcycleAlreadyExistsException(string plate)
            : base($"A motorcycle with the plate '{plate}' already exists.")
        {
        }
    }
}