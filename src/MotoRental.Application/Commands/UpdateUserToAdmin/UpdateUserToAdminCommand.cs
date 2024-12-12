using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MotoRental.Core.Entities;

namespace MotoRental.Application.Commands.UpdateUserToAdmin
{
    public class UpdateUserToAdminCommand : IRequest<Unit>
    {
        public string UserId { get; set; }
        public bool IsAdmin { get; set; }

        public UpdateUserToAdminCommand(string userId, bool isAdmin)
        {
            UserId = userId;
            IsAdmin = isAdmin;
        }

    }
}