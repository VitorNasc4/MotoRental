using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MotoRental.Core.Entities;

namespace MotoRental.Core.Repositories
{
    public interface IMotorcycleRepository
    {
        Task<Motorcycle> GetMotorcycleByIdAsync(string id);
        Task<List<Motorcycle>> GetMotorcyclesByPlateAsync(string plate);
        Task<bool> MotorcyclePlateAlreadyExistsAsync(string plate);
        Task AddAsync(Motorcycle motorcycle);
        Task RemoveAsync(Motorcycle motorcycle);
        Task SaveChangesAsync();
    }
}
