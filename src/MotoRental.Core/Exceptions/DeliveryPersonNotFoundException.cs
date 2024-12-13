using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.Core.Exceptions
{
    public class DeliveryPersonNotFoundException : Exception
    {
        public DeliveryPersonNotFoundException(string id)
            : base($"Entregador com id '{id}' não foi encontrado")
        {
        }
    }
}