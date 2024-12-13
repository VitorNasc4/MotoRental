using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MotoRental.Infrastructure.Persistence;

public class DatabaseFixture : IDisposable
{
    public MotoRentalDbContext DbContext { get; private set; }
    private readonly InMemoryDatabaseRoot _databaseRoot;

    public DatabaseFixture()
    {
        _databaseRoot = new InMemoryDatabaseRoot();

        var options = new DbContextOptionsBuilder<MotoRentalDbContext>()
            .UseInMemoryDatabase("MotoRentalDatabase", _databaseRoot)
            .Options;

        DbContext = new MotoRentalDbContext(options);

        DbContext.Database.EnsureCreated();
    }

    public void ClearDatabase()
    {
        DbContext.Motorcycles.RemoveRange(DbContext.Motorcycles);
        DbContext.DeliveryPersons.RemoveRange(DbContext.DeliveryPersons);
        DbContext.Rentals.RemoveRange(DbContext.Rentals);
        DbContext.SaveChanges();
    }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}
