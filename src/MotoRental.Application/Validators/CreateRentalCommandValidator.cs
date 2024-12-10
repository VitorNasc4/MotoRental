using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MotoRental.Application.Commands.CreateUser;
using FluentValidation;
using MotoRental.Core.Entities;
using MotoRental.Application.Commands.CreateRental;

namespace MotoRental.Application.Validators
{
    public class CreateRentalCommandValidator : AbstractValidator<CreateRentalCommand>
    {
        public CreateRentalCommandValidator()
        {
            RuleFor(u => u.entregador_id)
                .NotNull()
                .NotEmpty()
                .WithMessage("Delivery Person Id is required");
 
            RuleFor(u => u.moto_id)
                .NotNull()
                .NotEmpty()
                .WithMessage("Motorcycle Id is required");

            RuleFor(u => u.data_inicio)
                .NotNull()
                .NotEmpty()
                .WithMessage("Initial date is required");

            RuleFor(u => u.data_previsao_termino)
                .NotNull()
                .NotEmpty()
                .WithMessage("Expected final date is required");

            RuleFor(u => u.plano)
                .Must(Rental.IsValidPlan)
                .WithMessage("Invalid plan");
        }
    }
}