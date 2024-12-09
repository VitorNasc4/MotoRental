using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotoRental.Application.Commands.CreateUser;
using MotoRental.Core.Entities;
using MotoRental.Core.Repositories;
using MotoRental.Core.Services;
using MotoRental.Test.Mocks;
using Moq;
using Xunit;

namespace MotoRental.Test.Application.Commands
{
    public class CreateUserCommandHandlerTest
    {
        [Fact]
        public async Task InputDataIsOk_Executed_ReturnUserId()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var authServiceMock = new Mock<IAuthService>();

            var createUserCommand = new CreateUserCommand
            {
                BirthDate = DateTime.UtcNow,
                Email = "email@email.com",
                FullName = "Teste",
                Password = "Teste",
            };

            var sut = new CreateUserCommandHandler(userRepositoryMock.Object, authServiceMock.Object);
            var id = await sut.Handle(createUserCommand, new CancellationToken());

            Assert.True(id >= 0);
            userRepositoryMock.Verify(pr => pr.AddAsync(It.IsAny<User>()), Times.Once);
            authServiceMock.Verify(a => a.ComputeSha256Hash(createUserCommand.Password), Times.Once);
        }
    }
}