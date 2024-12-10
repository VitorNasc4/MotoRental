using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.Core.Exceptions
{
    public class RentalBadRequestException : Exception
    {
        public RentalBadRequestException(string message)
            : base(message)
        {
        }
    }
}