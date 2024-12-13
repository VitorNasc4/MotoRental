using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MotoRental.Application.Commands.CreateMotorcycle;
using MotoRental.Application.Commands.CreateUser;
using MotoRental.Application.Commands.LoginUser;
using MotoRental.Application.InputModels;
using MotoRental.Application.ViewModels;
using MotoRental.Core.Entities;
using MotoRental.Infrastructure.AuthServices;
using MotoRental.Test.Integration.Factory;
using Xunit;

namespace MotoRental.Test.Integration
{
    public class MotorcycleIntegrationTest : IClassFixture<DatabaseFixture>, IAsyncLifetime
    {
        private readonly DatabaseFixture _fixture;
        private readonly MotoRentalWebApplicationFactory _factory;
        public MotorcycleIntegrationTest(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _factory = new MotoRentalWebApplicationFactory(_fixture.DbContext);

            _fixture.ClearDatabase();
        }
        [Fact]
        public async Task GET_GetMotorcycleById_OnSuccess()
        {
            var dbContext = _fixture.DbContext;
            using var client = _factory.CreateClient();

            var motorcycle = new Motorcycle("2024", "dummy modelo", "dummy placa");
            dbContext.Motorcycles.Add(motorcycle);
            await dbContext.SaveChangesAsync();

            var result = await client.GetAsync($"api/motorcycles/{motorcycle.Id}");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
        [Fact]
        public async Task GET_GetMotorcycleById_OnFailure()
        {
            using var client = _factory.CreateClient();

            var result = await client.GetAsync($"api/motorcycles/dummyId");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);

        }
        [Fact]
        public async Task GET_GetMotorcycleByPlate_OnSuccess()
        {
            var dbContext = _fixture.DbContext;
            using var client = _factory.CreateClient();

            var motorcycle = new Motorcycle("2024", "dummy modelo", "dummy placa");
            dbContext.Motorcycles.Add(motorcycle);
            await dbContext.SaveChangesAsync();

            var result = await client.GetAsync($"api/motorcycles/plate?plate={motorcycle.Plate}");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
        [Fact]
        public async Task GET_GetMotorcycleByPlate_WhenNoContent()
        {
            using var client = _factory.CreateClient();

            var result = await client.GetAsync($"api/motorcycles/plate?plate=dummyPlate");

            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        }
        [Fact]
        public async Task PUT_ChangePlate_OnSuccess()
        {
            var dbContext = _fixture.DbContext;
            using var client = _factory.CreateClient();

            var motorcycle = new Motorcycle("2024", "dummy modelo", "dummy placa");
            dbContext.Motorcycles.Add(motorcycle);
            await dbContext.SaveChangesAsync();

            var updateMotorcycleInputModel = new UpdateMotorcycleInputModel { placa = "dummmy nova placa" };

            var result = await client.PutAsJsonAsync($"api/motorcycles/{motorcycle.Id}/plate", updateMotorcycleInputModel);

            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);

            dbContext.ChangeTracker.Clear();

            var resultData = await dbContext.Motorcycles.Where(m => m.Id == motorcycle.Id).FirstOrDefaultAsync();
            Assert.Equal(updateMotorcycleInputModel.placa, resultData!.Plate);
        }
        [Fact]
        public async Task PUT_ChangePlate_OnConflict()
        {
            var dbContext = _fixture.DbContext;
            using var client = _factory.CreateClient();

            var motorcycle1 = new Motorcycle("2024", "dummy modelo", "dummy placa");
            dbContext.Motorcycles.Add(motorcycle1);
            var motorcycle2 = new Motorcycle("2024", "dummy modelo 2", "dummy placa 2");
            dbContext.Motorcycles.Add(motorcycle2);
            await dbContext.SaveChangesAsync();

            var updateMotorcycleInputModel = new UpdateMotorcycleInputModel { placa = motorcycle1.Plate };

            var result = await client.PutAsJsonAsync($"api/motorcycles/{motorcycle2.Id}/plate", updateMotorcycleInputModel);

            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
        }
        [Fact]
        public async Task PUT_ChangePlate_OnFailure()
        {
            using var client = _factory.CreateClient();

            var updateMotorcycleInputModel = new UpdateMotorcycleInputModel { placa = "dummmy nova placa" };

            var result = await client.PutAsJsonAsync($"api/motorcycles/dummyId/plate", updateMotorcycleInputModel);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public async Task DELETE_DeleteMotorcycle_OnSuccess()
        {
            var dbContext = _fixture.DbContext;
            using var client = _factory.CreateClient();

            var motorcycle = new Motorcycle("2024", "dummy modelo", "dummy placa");
            dbContext.Motorcycles.Add(motorcycle);
            await dbContext.SaveChangesAsync();

            var result = await client.DeleteAsync($"api/motorcycles/{motorcycle.Id}");
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);

            dbContext.ChangeTracker.Clear();

            var resultData = await dbContext.Motorcycles.Where(m => m.Id == motorcycle.Id).FirstOrDefaultAsync();
            Assert.Null(resultData);
        }
        [Fact]
        public async Task DELETE_DeleteMotorcycle_OnFaiulure()
        {
            using var client = _factory.CreateClient();

            var result = await client.DeleteAsync($"api/motorcycles/dummyId");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public async Task DELETE_DeleteMotorcycle_OnConflicts()
        {
            var dbContext = _fixture.DbContext;
            using var client = _factory.CreateClient();

            var deliveryPerson = new DeliveryPerson("Dummy Name", "34283411000153", DateTime.Today.AddYears(-18), "123", CNH_Types.Type_A, "");
            dbContext.DeliveryPersons.Add(deliveryPerson);
            var motorcycle = new Motorcycle("2024", "dummy modelo", "dummy placa");
            dbContext.Motorcycles.Add(motorcycle);
            var rental = new Rental(motorcycle.Id, deliveryPerson.Id, PlanTypes.SevenDays, DateTime.Today.AddDays(1), DateTime.Today.AddDays(PlanTypes.SevenDays + 1), DateTime.Today.AddDays(PlanTypes.SevenDays + 1) );
            dbContext.Rentals.Add(rental);
            await dbContext.SaveChangesAsync();

            var result = await client.DeleteAsync($"api/motorcycles/{motorcycle.Id}");
            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
        }
        public async Task InitializeAsync()
        {
            await _fixture.DbContext.Database.EnsureDeletedAsync();
            await _fixture.DbContext.Database.EnsureCreatedAsync();
        }

        public Task DisposeAsync() => Task.CompletedTask;
    }
}