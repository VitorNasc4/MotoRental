﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MotoRental.Core.Entities;

namespace MotoRental.Core.Repositories
{
    public interface IRentalRepository
    {
        Task AddAsync(Rental rental);
        Task<Rental> GetRentalByIdAsync(int id);
        Task SaveChangesAsync();
    }
}