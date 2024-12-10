using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotoRental.Core.Entities;
using MotoRental.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MotoRental.Infrastructure.Persistence.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly MotoRentalDbContext _dbContext;

        public RentalRepository(MotoRentalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Rental rental)
        {
            await _dbContext.Rentals.AddAsync(rental);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Rental> GetRentalByIdAsync(int id)
        {
            var rental = await _dbContext.Rentals
                .SingleOrDefaultAsync(dp => dp.Id == id);

            return rental;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}