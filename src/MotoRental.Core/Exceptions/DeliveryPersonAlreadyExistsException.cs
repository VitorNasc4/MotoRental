using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.Core.Exceptions
{
    public class DeliveryPersonAlreadyExistsException : Exception
    {
        public DeliveryPersonAlreadyExistsException(string cnh, string cnpj)
            : base($"A delivery person with the cnh '{cnh}' or cnpj '{cnpj}' already exists.")
        {
        }
    }
}