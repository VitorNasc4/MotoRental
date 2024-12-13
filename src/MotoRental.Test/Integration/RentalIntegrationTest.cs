using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MotoRental.Application.Commands.CreateMotorcycle;
using MotoRental.Application.Commands.CreateRental;
using MotoRental.Application.Commands.CreateUser;
using MotoRental.Application.Commands.LoginUser;
using MotoRental.Application.Commands.UpdateRental;
using MotoRental.Application.InputModels;
using MotoRental.Application.ViewModels;
using MotoRental.Core.Entities;
using MotoRental.Infrastructure.AuthServices;
using MotoRental.Test.Integration.Factory;
using Xunit;

namespace MotoRental.Test.Integration
{
    public class RentalIntegrationTest : IClassFixture<DatabaseFixture>, IAsyncLifetime
    {
        private readonly DatabaseFixture _fixture;
        private readonly MotoRentalWebApplicationFactory _factory;
        private Motorcycle dummyMotorcycle = new Motorcycle("2024", "dummy modelo", "dummy placa");
        private DeliveryPerson dummyDeliveryPerson = new DeliveryPerson("Dummy Name", "34283411000153", DateTime.Today.AddYears(-18), "123", CNH_Types.Type_A, "");
        public RentalIntegrationTest(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _factory = new MotoRentalWebApplicationFactory(_fixture.DbContext);

            _fixture.ClearDatabase();
        }
        [Fact]
        public async Task GET_GetRentalById_OnSuccess()
        {
            var dbContext = _fixture.DbContext;
            using var client = _factory.CreateClient();

            dbContext.Motorcycles.Add(dummyMotorcycle);
            dbContext.DeliveryPersons.Add(dummyDeliveryPerson);
            var rental = new Rental(dummyMotorcycle.Id, dummyDeliveryPerson.Id, PlanTypes.SevenDays, DateTime.Today.AddDays(1), DateTime.Today.AddDays(PlanTypes.SevenDays + 1), DateTime.Today.AddDays(PlanTypes.SevenDays + 1) );
            dbContext.Rentals.Add(rental);
            await dbContext.SaveChangesAsync();

            var result = await client.GetAsync($"locacao/{rental.Id}");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
        [Fact]
        public async Task GET_GetRentalById_OnFailure()
        {

            using var client = _factory.CreateClient();

            var result = await client.GetAsync($"locacao/dummyId");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        [Fact]
        public async Task POST_CreateRental_OnSuccess()
        {
            var dbContext = _fixture.DbContext;
            using var client = _factory.CreateClient();

            dbContext.Motorcycles.Add(dummyMotorcycle);
            dbContext.DeliveryPersons.Add(dummyDeliveryPerson);
            await dbContext.SaveChangesAsync();

            var createRentalCommand = new CreateRentalCommand
            {
                moto_id = dummyMotorcycle.Id,
                entregador_id = dummyDeliveryPerson.Id,
                plano = PlanTypes.SevenDays,
                data_inicio = DateTime.Today.AddDays(1),
                data_termino = DateTime.Today.AddDays(PlanTypes.SevenDays + 1),
                data_previsao_termino = DateTime.Today.AddDays(PlanTypes.SevenDays + 1)
            };

            var result = await client.PostAsJsonAsync($"locacao/", createRentalCommand);

            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);

            var rental = await dbContext.Rentals.FirstOrDefaultAsync();

            Assert.Equal(createRentalCommand.moto_id, rental!.MotorcycleId);
            Assert.Equal(createRentalCommand.entregador_id, rental!.DeliveryPersonId);
            Assert.Equal(createRentalCommand.plano, rental!.PlanDays);
            Assert.Equal(createRentalCommand.data_inicio.ToUniversalTime(), rental!.StartDate.ToUniversalTime());
            Assert.Equal(createRentalCommand.data_termino.ToUniversalTime(), rental!.EndDate.ToUniversalTime());
            Assert.Equal(createRentalCommand.data_previsao_termino.ToUniversalTime(), rental!.ExpectedReturnDate.ToUniversalTime());
        }
        [Fact]
        public async Task POST_CreateRental_OnFailure_WrongInitialDate()
        {
            var dbContext = _fixture.DbContext;
            using var client = _factory.CreateClient();

            dbContext.Motorcycles.Add(dummyMotorcycle);
            dbContext.DeliveryPersons.Add(dummyDeliveryPerson);
            await dbContext.SaveChangesAsync();

            var createRentalCommand = new CreateRentalCommand
            {
                moto_id = dummyMotorcycle.Id,
                entregador_id = dummyDeliveryPerson.Id,
                plano = PlanTypes.SevenDays,
                data_inicio = DateTime.Today,
                data_termino = DateTime.Today.AddDays(PlanTypes.SevenDays + 1),
                data_previsao_termino = DateTime.Today.AddDays(PlanTypes.SevenDays + 1)
            };

            var result = await client.PostAsJsonAsync($"locacao/", createRentalCommand);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
        [Fact]
        public async Task POST_CreateRental_OnFailure_WrongEndDate()
        {
            var dbContext = _fixture.DbContext;
            using var client = _factory.CreateClient();

            dbContext.Motorcycles.Add(dummyMotorcycle);
            dbContext.DeliveryPersons.Add(dummyDeliveryPerson);
            await dbContext.SaveChangesAsync();

            var createRentalCommand = new CreateRentalCommand
            {
                moto_id = dummyMotorcycle.Id,
                entregador_id = dummyDeliveryPerson.Id,
                plano = PlanTypes.SevenDays,
                data_inicio = DateTime.Today.AddDays(1),
                data_termino = DateTime.Today,
                data_previsao_termino = DateTime.Today.AddDays(PlanTypes.SevenDays + 1)
            };

            var result = await client.PostAsJsonAsync($"locacao/", createRentalCommand);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
        [Fact]
        public async Task POST_CreateRental_OnFailure_ExpectedEndDateBeforeStartDate()
        {
            var dbContext = _fixture.DbContext;
            using var client = _factory.CreateClient();

            dbContext.Motorcycles.Add(dummyMotorcycle);
            dbContext.DeliveryPersons.Add(dummyDeliveryPerson);
            await dbContext.SaveChangesAsync();

            var createRentalCommand = new CreateRentalCommand
            {
                moto_id = dummyMotorcycle.Id,
                entregador_id = dummyDeliveryPerson.Id,
                plano = PlanTypes.SevenDays,
                data_inicio = DateTime.Today.AddDays(1),
                data_termino = DateTime.Today.AddDays(PlanTypes.SevenDays + 1),
                data_previsao_termino = DateTime.Today
            };

            var result = await client.PostAsJsonAsync($"locacao/", createRentalCommand);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
        [Fact]
        public async Task POST_CreateRental_OnFailure_ExpectedEndDateAfterEndDate()
        {
            var dbContext = _fixture.DbContext;
            using var client = _factory.CreateClient();

            dbContext.Motorcycles.Add(dummyMotorcycle);
            dbContext.DeliveryPersons.Add(dummyDeliveryPerson);
            await dbContext.SaveChangesAsync();

            var createRentalCommand = new CreateRentalCommand
            {
                moto_id = dummyMotorcycle.Id,
                entregador_id = dummyDeliveryPerson.Id,
                plano = PlanTypes.SevenDays,
                data_inicio = DateTime.Today.AddDays(1),
                data_termino = DateTime.Today.AddDays(PlanTypes.SevenDays + 1),
                data_previsao_termino = DateTime.Today.AddDays(PlanTypes.SevenDays + 2)
            };

            var result = await client.PostAsJsonAsync($"locacao/", createRentalCommand);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
        [Fact]
        public async Task POST_CreateRental_OnFailure_InexistentMotorcycleId()
        {
            using var client = _factory.CreateClient();

            var createRentalCommand = new CreateRentalCommand
            {
                moto_id = "dummyId",
                entregador_id = dummyDeliveryPerson.Id,
                plano = PlanTypes.SevenDays,
                data_inicio = DateTime.Today.AddDays(1),
                data_termino = DateTime.Today.AddDays(PlanTypes.SevenDays + 1),
                data_previsao_termino = DateTime.Today.AddDays(PlanTypes.SevenDays + 1)
            };

            var result = await client.PostAsJsonAsync($"locacao/", createRentalCommand);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
        [Fact]
        public async Task POST_CreateRental_OnFailure_InexistentDeliveryPersonId()
        {
            using var client = _factory.CreateClient();

            var createRentalCommand = new CreateRentalCommand
            {
                moto_id = dummyMotorcycle.Id,
                entregador_id = "dummyDeliveryPersonId",
                plano = PlanTypes.SevenDays,
                data_inicio = DateTime.Today.AddDays(1),
                data_termino = DateTime.Today.AddDays(PlanTypes.SevenDays + 1),
                data_previsao_termino = DateTime.Today.AddDays(PlanTypes.SevenDays + 1)
            };

            var result = await client.PostAsJsonAsync($"locacao/", createRentalCommand);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
        [Fact]
        public async Task PUT_RentalReturn_OnSuccess()
        {
            var dbContext = _fixture.DbContext;
            using var client = _factory.CreateClient();

            dbContext.Motorcycles.Add(dummyMotorcycle);
            dbContext.DeliveryPersons.Add(dummyDeliveryPerson);
            var rental = new Rental(dummyMotorcycle.Id, dummyDeliveryPerson.Id, PlanTypes.SevenDays, DateTime.Today.AddDays(1), DateTime.Today.AddDays(PlanTypes.SevenDays + 1), DateTime.Today.AddDays(PlanTypes.SevenDays + 1) );
            dbContext.Rentals.Add(rental);
            await dbContext.SaveChangesAsync();

            var updateRentalInputModel = new UpdateRentalInputModel{ data_devolucao = rental.ExpectedReturnDate };

            var result = await client.PutAsJsonAsync($"locacao/{rental.Id}/devolucao", updateRentalInputModel);

            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);

            dbContext.ChangeTracker.Clear();

            var resultData = dbContext.Rentals.FirstOrDefault();
            Assert.Equal(rental.ExpectedReturnDate, resultData!.ActualReturnDate);
        }
        [Fact]
        public async Task PUT_RentalReturn_OnFailure()
        {
            var dbContext = _fixture.DbContext;
            using var client = _factory.CreateClient();

            dbContext.Motorcycles.Add(dummyMotorcycle);
            dbContext.DeliveryPersons.Add(dummyDeliveryPerson);
            var rental = new Rental(dummyMotorcycle.Id, dummyDeliveryPerson.Id, PlanTypes.SevenDays, DateTime.Today.AddDays(1), DateTime.Today.AddDays(PlanTypes.SevenDays + 1), DateTime.Today.AddDays(PlanTypes.SevenDays + 1) );
            dbContext.Rentals.Add(rental);
            await dbContext.SaveChangesAsync();

            var updateRentalInputModel = new UpdateRentalInputModel{ data_devolucao = DateTime.Today };

            var result = await client.PutAsJsonAsync($"locacao/{rental.Id}/devolucao", updateRentalInputModel);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
        public async Task InitializeAsync()
        {
            await _fixture.DbContext.Database.EnsureDeletedAsync();
            await _fixture.DbContext.Database.EnsureCreatedAsync();
        }

        public Task DisposeAsync() => Task.CompletedTask;
    }
}