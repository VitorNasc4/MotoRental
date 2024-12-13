using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.Core.Exceptions
{
    public class InvalidRentalException : Exception
    {
        public InvalidRentalException(int plan)
            : base($"O plano '{plan}' é inválido")
        {
        }
    }
}