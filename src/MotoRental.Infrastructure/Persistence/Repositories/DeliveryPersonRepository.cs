using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotoRental.Core.Entities;
using MotoRental.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MotoRental.Infrastructure.Persistence.Repositories
{
    public class DeliveryPersonRepository : IDeliveryPersonRepository
    {
        private readonly MotoRentalDbContext _dbContext;

        public DeliveryPersonRepository(MotoRentalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(DeliveryPerson deliveryPerson)
        {
            await _dbContext.DeliveryPersons.AddAsync(deliveryPerson);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeliveryPersonAlreadyExistsAsync(string cnh, string cnpj)
        {
            var deliveryPerson = await _dbContext.DeliveryPersons
                .AnyAsync(d => d.CNH_Number == cnh || d.CNPJ == cnpj);

            return deliveryPerson;
        }

        public async Task<DeliveryPerson> GetDeliveryPersonByIdAsync(int id)
        {
            var deliveryPerson = await _dbContext.DeliveryPersons
                .SingleOrDefaultAsync(dp => dp.Id == id);

            return deliveryPerson;
        }
    }
}