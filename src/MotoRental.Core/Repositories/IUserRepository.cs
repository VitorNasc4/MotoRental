using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MotoRental.Core.Entities;

namespace MotoRental.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(string id);
        Task<User> GetUserByEmailAndPasswordAsyn(string email, string passwordHash);
        Task AddAsync(User user);
    }
}
