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
                .WithMessage("Id do entregador é obrigatório");
 
            RuleFor(u => u.moto_id)
                .NotNull()
                .NotEmpty()
                .WithMessage("Id da moto é obrigatório");

            RuleFor(u => u.data_inicio)
                .NotNull()
                .NotEmpty()
                .WithMessage("Data de início é obrigatório");

            RuleFor(u => u.data_previsao_termino)
                .NotNull()
                .NotEmpty()
                .WithMessage("Previsão de término é obrigatório");

            RuleFor(u => u.plano)
                .Must(Rental.IsValidPlan)
                .WithMessage($"Plano inválido, os planos disponíveis são: {PlanTypes.SevenDays}, {PlanTypes.FifteenDays}, {PlanTypes.ThirtyDays}, {PlanTypes.FortyFiveDays} e {PlanTypes.FiftyDays} diárias");
        }
    }
}