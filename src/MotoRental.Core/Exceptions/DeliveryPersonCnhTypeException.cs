using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.Core.Exceptions
{
    public class DeliveryPersonCnhTypeException : Exception
    {
        public DeliveryPersonCnhTypeException(string id)
            : base($"O entregador com id '{id}' não tem o tipo de permissão necessária para dirigir uma moto")
        {
        }
    }
}