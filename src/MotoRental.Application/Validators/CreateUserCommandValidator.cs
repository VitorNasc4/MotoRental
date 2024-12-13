using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MotoRental.Application.Commands.CreateUser;
using FluentValidation;
using MotoRental.Core.Entities;

namespace MotoRental.Application.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(u => u.Email)
                .EmailAddress()
                .WithMessage("Email inválido");

            RuleFor(u => u.Password)
                .Must(ValidPassword)
                .WithMessage("A senha precisa ter pelo menos 8 caracteres");

            RuleFor(u => u.FullName)
                .NotNull()
                .NotEmpty()
                .WithMessage("Nome é obrigatório");
        }

        public static bool ValidPassword(string password)
        {
            return password.Length >= 8;
        }
    }
}