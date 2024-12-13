using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.Core.Exceptions
{
    public class MotorcycleAlreadyExistsException : Exception
    {
        public MotorcycleAlreadyExistsException(string plate)
            : base($"Placa '{plate}' já cadastrada")
        {
        }
    }
}