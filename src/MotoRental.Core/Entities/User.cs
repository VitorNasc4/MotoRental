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
            Role = role ?? RoleTypes.Delivery;
        }

        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Role { get; private set; }

        public void SetAdmin(bool isAdmin)
        {
            if (isAdmin)
            {
                Role = RoleTypes.Admin;
                return;
            }

            Role = RoleTypes.Delivery;
        }

        public static bool IsValidRoleType(string role)
        {
            var formatedRole = role.ToUpper();
            if (formatedRole != RoleTypes.Admin && formatedRole != RoleTypes.Delivery)
            {
                return false;
            }

            return true;
        }

    }

    public static class RoleTypes
    {
        public const string Admin = "ADMIN";
        public const string Delivery = "DELIVERY";

    }
}
