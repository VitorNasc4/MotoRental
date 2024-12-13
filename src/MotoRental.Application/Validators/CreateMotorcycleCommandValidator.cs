using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MotoRental.Application.Commands.CreateUser;
using FluentValidation;
using MotoRental.Application.Commands.CreateMotorcycle;

namespace MotoRental.Application.Validators
{
    public class CreateMotorcycleCommandValidator : AbstractValidator<CreateMotorcycleCommand>
    {
        public CreateMotorcycleCommandValidator()
        {
            RuleFor(u => u.ano)
                .NotNull()
                .NotEmpty()
                .WithMessage("Ano é obrigatório");
 
            RuleFor(u => u.modelo)
                .NotNull()
                .NotEmpty()
                .WithMessage("Modelo é obrigatório");

            RuleFor(u => u.placa)
                .NotNull()
                .NotEmpty()
                .WithMessage("Placa é obrigatório");
        }
    }
}