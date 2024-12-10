using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MotoRental.Core.Entities;

namespace MotoRental.Core.Repositories
{
    public interface IDeliveryPersonRepository
    {
        Task<bool> DeliveryPersonAlreadyExistsAsync(string cnh, string cnpj);
        Task AddAsync(DeliveryPerson deliveryPerson);
        Task<DeliveryPerson> GetDeliveryPersonByIdAsync(int id);
    }
}
