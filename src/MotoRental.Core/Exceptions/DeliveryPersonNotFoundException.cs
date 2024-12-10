using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.Core.Exceptions
{
    public class DeliveryPersonNotFoundException : Exception
    {
        public DeliveryPersonNotFoundException(int id)
            : base($"A delivery person with id '{id}' not found.")
        {
        }
    }
}