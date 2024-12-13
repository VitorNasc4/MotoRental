using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.Core.Exceptions
{
    public class UserIdNotFoundException : Exception
    {
        public UserIdNotFoundException(string id)
            : base($"Usuário com id '{id}' não foi encontrado")
        {
        }
    }
}