using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MotoRental.Core.Entities;

namespace MotoRental.Application.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<int>
    {
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public static User ToEntity(CreateUserCommand command, string passwordHash = null)
        {
            var password = passwordHash is not null ? passwordHash : command.Password;
            return new User(command.FullName, command.Email, password, RoleTypes.Delivery);
        }

    }
}