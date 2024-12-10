using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.Core.Exceptions
{
    public class RentalNotFoundException : Exception
    {
        public RentalNotFoundException(int id)
            : base($"A rental with id '{id}' not found.")
        {
        }
    }
}