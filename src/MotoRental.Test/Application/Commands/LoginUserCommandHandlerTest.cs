using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotoRental.Application.Commands.LoginUser;
using MotoRental.Core.Entities;
using MotoRental.Core.Repositories;
using MotoRental.Core.Services;
using MotoRental.Test.Mocks;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace MotoRental.Test.Application.Commands
{
    public class LoginUserCommandHandlerTest
    {
        [Fact]
        public async Task InputDataIsOk_Executed_ReturnLoginUserViewModel()
        {
            var user = UserMocks.GetValidClientUser();
            var loginUserCommand = new LoginUserCommand
            {
                Email = user.Email,
                Password = user.Password,
            };
            var passwordHash = "dummyHashPassword";
            var token = "dummyToken";

            var authServiceMock = new Mock<IAuthService>();
            authServiceMock
                .Setup(a => a.ComputeSha256Hash(user.Password))
                .Returns(passwordHash);
            authServiceMock
                .Setup(a => a.GenerateJWTToken(user.Email, user.Role))
                .Returns(token);

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock
                .Setup(ur => ur.GetUserByEmailAndPasswordAsyn(loginUserCommand.Email, passwordHash))
                .ReturnsAsync(user);


            var sut = new LoginUserCommandHandler(authServiceMock.Object, userRepositoryMock.Object);
            var result = await sut.Handle(loginUserCommand, new CancellationToken());

            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(token, result.Token);
            userRepositoryMock.Verify(pr => pr.GetUserByEmailAndPasswordAsyn(loginUserCommand.Email, passwordHash), Times.Once);
            authServiceMock.Verify(a => a.ComputeSha256Hash(user.Password), Times.Once);
            authServiceMock.Verify(a => a.GenerateJWTToken(user.Email, user.Role), Times.Once);
        }
        [Fact]
        public async Task InputDataIsNotOk_Executed_ReturnNull()
        {
            var user = UserMocks.GetValidClientUser();
            var loginUserCommand = new LoginUserCommand
            {
                Email = user.Email,
                Password = user.Password,
            };
            var passwordHash = "dummyHashPassword";

            var authServiceMock = new Mock<IAuthService>();
            authServiceMock
                .Setup(a => a.ComputeSha256Hash(user.Password))
                .Returns(passwordHash);

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock
                .Setup(ur => ur.GetUserByEmailAndPasswordAsyn(loginUserCommand.Email, passwordHash))
                .ReturnsAsync((User)null!);


            var sut = new LoginUserCommandHandler(authServiceMock.Object, userRepositoryMock.Object);
            var result = await sut.Handle(loginUserCommand, new CancellationToken());

            Assert.Null(result);

            userRepositoryMock.Verify(pr => pr.GetUserByEmailAndPasswordAsyn(loginUserCommand.Email, passwordHash), Times.Once);
            authServiceMock.Verify(a => a.ComputeSha256Hash(user.Password), Times.Once);
            authServiceMock.Verify(a => a.GenerateJWTToken(user.Email, user.Role), Times.Never);
        }
    }
}