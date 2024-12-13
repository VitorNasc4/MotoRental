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
    public class DeliveryPersonIntegrationTest
    {
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService;
        private Dictionary<string, string?> inMemorySettings = new Dictionary<string, string?> {
            {"Jwt:Issuer", "teste1"},
            {"Jwt:Audience","teste2"},
            {"Jwt:Key","teste3"}
        };
        public DeliveryPersonIntegrationTest()
        {
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();;
            _authService = new AuthService(_configuration);
        }
        [Fact]
        public async Task POST_CreateDeliveryPerson_OnSucces()
        {
            var app = new MotoRentalWebApplicationFactory();
            using var client = app.CreateClient();
            using var dbContext = app.CreateDbContext();

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
            var app = new MotoRentalWebApplicationFactory();
            using var client = app.CreateClient();
            using var dbContext = app.CreateDbContext();

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
            var app = new MotoRentalWebApplicationFactory();
            using var client = app.CreateClient();
            using var dbContext = app.CreateDbContext();

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
            var app = new MotoRentalWebApplicationFactory();
            using var client = app.CreateClient();


            var createUserCommand1 = new CreateUserCommand { FullName = "Dummy Name", Email = "dummyEmail@email.com", Password = "teste123"};
            await client.PostAsJsonAsync("api/users", createUserCommand1);

            var createUserCommand2 = new CreateUserCommand { FullName = "Dummy Name 2", Email = "dummyEmail@email.com", Password = "teste1234"};
            var result = await client.PostAsJsonAsync("api/users", createUserCommand1);

            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
        }
    }
}