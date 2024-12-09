using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MotoRental.Application.Commands.CreateUser;
using FluentValidation;

namespace MotoRental.Application.Validators
{
    public class CreateUserAdminCommandValidator : AbstractValidator<CreateUserAdminCommand>
    {
        public CreateUserAdminCommandValidator()
        {
            RuleFor(u => u.Email)
                .EmailAddress()
                .WithMessage("Invalid e-mail");

            RuleFor(u => u.Password)
                .Must(ValidPassword)
                .WithMessage("Password must be at least 8 characters long");

            RuleFor(u => u.FullName)
                .NotNull()
                .NotEmpty()
                .WithMessage("Name is required");

            RuleFor(u => u.Role)
                .Must(ValidRole)
                .WithMessage("Allowed roles are 'admin' and 'delivery'");
        }

        public static bool ValidPassword(string password)
        {
            return password.Length >= 8;
        }
        public static bool ValidRole(string role)
        {
            if (role != "delivery" && role != "admin")
            {
                return false;
            }
            return true;
        }
    }
}