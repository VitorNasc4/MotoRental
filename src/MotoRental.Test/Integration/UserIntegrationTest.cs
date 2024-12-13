using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MotoRental.Application.Commands.LoginUser;
using MotoRental.Test.Integration.Factory;
using Xunit;

namespace MotoRental.Test.Integration
{
    public class UserIntegrationTest
    {
        [Fact]
        public async Task PUT_Login_OnSucces()
        {
            var app = new MotoRentalWebApplicationFactory();

            var user = new LoginUserCommand { Email = "teste@email.com", Password = "teste123"};

            using var client = app.CreateClient();

            var result = await client.PostAsJsonAsync("/user/login", user);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
    }
}