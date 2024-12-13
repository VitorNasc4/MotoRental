using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MotoRental.Infrastructure.Persistence;

namespace MotoRental.Test.Integration.Factory
{
    public class MotoRentalWebApplicationFactory:WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var root = new InMemoryDatabaseRoot();
            
            builder.ConfigureServices( services =>
            {
                services.RemoveAll(typeof (DbContextOptions<MotoRentalDbContext>));
                services.AddDbContext<MotoRentalDbContext>(options =>
                    options.UseInMemoryDatabase("MotoRentalDatabase", root));
                
            });
            base.ConfigureWebHost(builder);
        }
    }
}