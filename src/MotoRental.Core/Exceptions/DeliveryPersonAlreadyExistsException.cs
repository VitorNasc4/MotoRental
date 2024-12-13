using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.Core.Exceptions
{
    public class DeliveryPersonAlreadyExistsException : Exception
    {
        public DeliveryPersonAlreadyExistsException(string cnh, string cnpj)
            : base($"Já existe um registro com a CNH '{cnh}' ou o CNPJ '{cnpj}'")
        {
        }
    }
}