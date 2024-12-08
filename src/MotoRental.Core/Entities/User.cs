using System;
using System.Collections.Generic;
using MotoRental.Core.DTOs;

namespace MotoRental.Core.Entities
{
    public class User : BaseEntity
    {
        public User(string fullName, string email, string password, string role)
        {
            FullName = fullName;
            Email = email;
            Password = password;
            Role = role ?? "delivery";
        }

        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Role { get; private set; }

        public static NotificationInfoDTO ToDTO(User user)
        {
            return new NotificationInfoDTO(user.Id, user.FullName, user.Email);
        }

    }
}
