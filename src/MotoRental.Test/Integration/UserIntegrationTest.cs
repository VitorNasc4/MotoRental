using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    public class UserIntegrationTest : IClassFixture<DatabaseFixture>, IAsyncLifetime
    {
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService;
        private readonly DatabaseFixture _fixture;
        private readonly MotoRentalWebApplicationFactory _factory;
        private Dictionary<string, string?> inMemorySettings = new Dictionary<string, string?> {
            {"Jwt:Issuer", "teste1"},
            {"Jwt:Audience","teste2"},
            {"Jwt:Key","teste3"}
        };
        public UserIntegrationTest(DatabaseFixture fixture)
        {
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();;
            _authService = new AuthService(_configuration);

            _fixture = fixture;
            _factory = new MotoRentalWebApplicationFactory(_fixture.DbContext);

            _fixture.ClearDatabase();
        }
        [Fact]
        public async Task PUT_Login_OnFailure()
        {
            using var client = _factory.CreateClient();

            var user = new LoginUserCommand { Email = "dummyEmail@email.com", Password = "teste123"};

            var result = await client.PutAsJsonAsync("api/users/login", user);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
        [Fact]
        public async Task PUT_Login_OnSuccess()
        {
            var dbContext = _fixture.DbContext;
            using var client = _factory.CreateClient();

            var password = "teste123";
            var hashedPassword = _authService.ComputeSha256Hash(password);

            var user = new User("Dummy Name", "dummyEmail@email.com", hashedPassword, RoleTypes.Delivery);

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            var loginUserCommand = new LoginUserCommand { Email = user.Email, Password = password};
            var result = await client.PutAsJsonAsync("api/users/login", loginUserCommand);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
        [Fact]
        public async Task POST_CreateUser_OnSucces()
        {
            using var client = _factory.CreateClient();

            var createUserCommand = new CreateUserCommand { FullName = "Dummy Name", Email = "dummyEmail@email.com", Password = "teste123"};
            var result1 = await client.PostAsJsonAsync("api/users", createUserCommand);

            Assert.Equal(HttpStatusCode.Created, result1.StatusCode);

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
        [Fact]
        public async Task GET_GetUserById_OnSuccess()
        {
            var dbContext = _fixture.DbContext;
            using var client = _factory.CreateClient();

            var user = new User("Dummy Name", "dummyEmail@email.com", "dummyPassword", RoleTypes.Delivery);

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            var result = await client.GetAsync($"api/users/{user.Id}");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            var resultData = await result.Content.ReadFromJsonAsync<UserViewModel>();
            Assert.Equal(user.FullName, resultData!.FullName);
            Assert.Equal(user.Email, resultData.Email);
        }
        [Fact]
        public async Task GET_GetUserById_OnFailure()
        {
            using var client = _factory.CreateClient();

            var result = await client.GetAsync($"api/users/dummyId");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);

        }
        [Fact]
        public async Task PUT_UpdateUserToAdmin_OnSuccess()
        {
            var dbContext = _fixture.DbContext;
            using var client = _factory.CreateClient();

            var user = new User("Dummy Name", "dummyEmail@email.com", "dummyPassword", RoleTypes.Delivery);

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            var updateUserInputModel = new UpdateUserInputModel { isAdmin = true };

            var result = await client.PutAsJsonAsync($"api/users/{user.Id}/admin", updateUserInputModel);

            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);

            dbContext.ChangeTracker.Clear();

            var resultData = await dbContext.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();
            Assert.Equal(RoleTypes.Admin, resultData!.Role);
        }

        public async Task InitializeAsync()
        {
            await _fixture.DbContext.Database.EnsureDeletedAsync();
            await _fixture.DbContext.Database.EnsureCreatedAsync();
        }

        public Task DisposeAsync() => Task.CompletedTask;
        }
}