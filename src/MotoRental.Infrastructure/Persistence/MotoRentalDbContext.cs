using MotoRental.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MotoRental.Infrastructure.Persistence
{
    public class MotoRentalDbContext : DbContext
    {
        public MotoRentalDbContext(DbContextOptions<MotoRentalDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Motorcycle> Motorcycles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
