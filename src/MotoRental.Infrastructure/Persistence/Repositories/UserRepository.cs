using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotoRental.Core.Entities;
using MotoRental.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MotoRental.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MotoRentalDbContext _dbContext;

        public UserRepository(MotoRentalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public Task<User> GetUserByEmailAndPasswordAsyn(string email, string passwordHash)
        {
            return _dbContext.Users
                .SingleOrDefaultAsync(u => u.Email == email && u.Password == passwordHash);
        }
        public Task<bool> CheckEmailExist(string email)
        {
            return _dbContext.Users
                .AnyAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var user = await _dbContext.Users
                .SingleOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}