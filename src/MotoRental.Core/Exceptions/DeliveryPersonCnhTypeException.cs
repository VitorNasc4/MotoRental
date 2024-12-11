using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.Core.Exceptions
{
    public class DeliveryPersonCnhTypeException : Exception
    {
        public DeliveryPersonCnhTypeException(string id)
            : base($"The delivery person with id '{id}' do not have the required CNH type to rent a motorcycle")
        {
        }
    }
}