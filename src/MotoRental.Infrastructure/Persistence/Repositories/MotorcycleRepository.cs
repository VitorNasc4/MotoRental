using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotoRental.Core.Entities;
using MotoRental.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MotoRental.Infrastructure.Persistence.Repositories
{
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly MotoRentalDbContext _dbContext;

        public MotorcycleRepository(MotoRentalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Motorcycle motorcycle)
        {
            await _dbContext.Motorcycles.AddAsync(motorcycle);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<Motorcycle> GetMotorcycleByIdAsync(int id)
        {
            var motorcycle = await _dbContext.Motorcycles
                .SingleOrDefaultAsync(m => m.Id == id);

            return motorcycle;
        }
        public async Task<List<Motorcycle>> GetMotorcyclesByPlateAsync(string plate)
        {
            if (plate is null)
            {
                return await _dbContext.Motorcycles.ToListAsync();
            }

            var motorcycle = await _dbContext.Motorcycles
                .Where(m => m.Plate.Contains(plate)).ToListAsync();

            return motorcycle;
        }
        public async Task<bool> MotorcyclePlateAlreadyExistsAsync(string plate)
        {
            var motorcycle = await _dbContext.Motorcycles
                .AnyAsync(m => m.Plate == plate);

            return motorcycle;
        }

        public async Task RemoveAsync(Motorcycle motorcycle)
        {
            _dbContext.Motorcycles.Remove(motorcycle);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}