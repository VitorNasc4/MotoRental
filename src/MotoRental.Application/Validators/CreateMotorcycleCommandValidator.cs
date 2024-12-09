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
            RuleFor(u => u.identificador)
                .NotNull()
                .NotEmpty()
                .WithMessage("identificador é obrigatório");

            RuleFor(u => u.ano)
                .NotNull()
                .NotEmpty()
                .WithMessage("ano é obrigatório");
 
            RuleFor(u => u.modelo)
                .NotNull()
                .NotEmpty()
                .WithMessage("modelo é obrigatório");

            RuleFor(u => u.placa)
                .NotNull()
                .NotEmpty()
                .WithMessage("placa é obrigatório");
        }
    }
}