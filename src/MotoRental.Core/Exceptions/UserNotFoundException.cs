using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.Core.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
            : base($"Email or password is incorrect")
        {
        }
    }
}