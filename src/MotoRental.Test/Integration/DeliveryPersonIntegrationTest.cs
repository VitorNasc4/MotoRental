using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MotoRental.Application.Commands.CreateDeliveryPerson;
using MotoRental.Application.Commands.CreateUser;
using MotoRental.Application.InputModels;
using MotoRental.Application.ViewModels;
using MotoRental.Core.Entities;
using MotoRental.Infrastructure.AuthServices;
using MotoRental.Test.Integration.Factory;
using Xunit;

namespace MotoRental.Test.Integration
{
    public class DeliveryPersonIntegrationTest : IClassFixture<DatabaseFixture>, IAsyncLifetime
    {
        private readonly DatabaseFixture _fixture;
        private readonly MotoRentalWebApplicationFactory _factory;
        public DeliveryPersonIntegrationTest(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _factory = new MotoRentalWebApplicationFactory(_fixture.DbContext);

            _fixture.ClearDatabase();
        }
        [Fact]
        public async Task POST_CreateDeliveryPerson_OnSucces()
        {
            var dbContext = _fixture.DbContext;
            using var client = _factory.CreateClient();

            var createDeliveryPersonCommand = new CreateDeliveryPersonCommand 
            { 
                nome = "Dummy Name",
                cnpj = "34283411000153",
                data_nascimento = DateTime.Today.AddYears(-18),
                numero_cnh = "123",
                tipo_cnh = "A"
            };
            var result = await client.PostAsJsonAsync("api/deliveryPerson", createDeliveryPersonCommand);

            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);

            var resultData = await dbContext.DeliveryPersons.FirstOrDefaultAsync();
            Assert.Equal(createDeliveryPersonCommand.nome, resultData!.FullName);
        }
        [Fact]
        public async Task POST_CreateDeliveryPerson_ReturnsBadRequest_ForUnder18YearsOld()
        {
            using var client = _factory.CreateClient();

            var createDeliveryPersonCommand = new CreateDeliveryPersonCommand 
            { 
                nome = "Dummy Name",
                cnpj = "123456789",
                data_nascimento = DateTime.Today,
                numero_cnh = "123",
                tipo_cnh = "A"
            };
            var result = await client.PostAsJsonAsync("api/deliveryPerson", createDeliveryPersonCommand);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
        [Fact]
        public async Task POST_CreateDeliveryPerson_ReturnsBadRequest_ForInvalidCNPJ()
        {
            using var client = _factory.CreateClient();

            var createDeliveryPersonCommand = new CreateDeliveryPersonCommand 
            { 
                nome = "Dummy Name",
                cnpj = "123456789",
                data_nascimento = DateTime.Today.AddYears(-18),
                numero_cnh = "123",
                tipo_cnh = "A"
            };
            var result = await client.PostAsJsonAsync("api/deliveryPerson", createDeliveryPersonCommand);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
        [Fact]
        public async Task POST_CreateUser_OnFailure()
        {
            using var client = _factory.CreateClient();

            var createUserCommand1 = new CreateUserCommand { FullName = "Dummy Name", Email = "dummyEmail@email.com", Password = "teste123"};
            await client.PostAsJsonAsync("api/users", createUserCommand1);

            var createUserCommand2 = new CreateUserCommand { FullName = "Dummy Name 2", Email = "dummyEmail@email.com", Password = "teste1234"};
            var result = await client.PostAsJsonAsync("api/users", createUserCommand1);

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