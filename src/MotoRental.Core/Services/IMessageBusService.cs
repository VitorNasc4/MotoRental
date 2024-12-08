using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoRental.Core.Services
{
    public interface IMessageBusService
    {
        void Publish(string queue, byte[] message);
    }
}