using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.Core.Exceptions
{
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException(string email)
            : base($"Email '{email}' already exists.")
        {
        }
    }
}