using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MotoRental.Core.Entities;

namespace MotoRental.Application.Commands.UserCommands.CreateUser
{
    public class CreateUserAdminCommand : IRequest<int>
    {
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public static User ToEntity(CreateUserAdminCommand command, string passwordHash = null)
        {
            var password = passwordHash is not null ? passwordHash : command.Password;
            return new User(command.FullName, command.Email, password, command.Role);
        }

    }
}