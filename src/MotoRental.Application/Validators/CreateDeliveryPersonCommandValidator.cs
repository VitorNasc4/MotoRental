using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MotoRental.Application.Commands.CreateUser;
using FluentValidation;
using MotoRental.Application.Commands.CreateDeliveryPerson;
using MotoRental.Core.Entities;

namespace MotoRental.Application.Validators
{
    public class CreateDeliveryPersonCommandValidator : AbstractValidator<CreateDeliveryPersonCommand>
    {
        public CreateDeliveryPersonCommandValidator()
        {
            RuleFor(dp => dp.nome)
                .NotNull()
                .NotEmpty()
                .WithMessage("Name is required");


            RuleFor(dp => dp.cnpj)
                .Must(ValidCNPJ)
                .WithMessage("Invalid CNPJ");

            RuleFor(dp => dp.data_nascimento)
                .NotNull()
                .NotEmpty()
                .WithMessage("Birthday is required");
            
            RuleFor(dp => dp.numero_cnh)
                .NotNull()
                .NotEmpty()
                .WithMessage("CNH number is required");
            
            RuleFor(dp => dp.tipo_cnh)
                .Must(DeliveryPerson.IsValidCNH_Type)
                .WithMessage("Invalid CNH type");

        }

        public static bool ValidCNPJ(string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj) || cnpj.Length != 14)
                return false;

            // if (new string(cnpj[0], cnpj.Length) == cnpj)
            //     return false;

            // int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            // int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            // string tempCnpj = cnpj.Substring(0, 12);
            // int soma = 0;

            // for (int i = 0; i < 12; i++)
            //     soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            // int resto = soma % 11;
            // int digito1 = resto < 2 ? 0 : 11 - resto;

            // tempCnpj += digito1;
            // soma = 0;

            // for (int i = 0; i < 13; i++)
            //     soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            // resto = soma % 11;
            // int digito2 = resto < 2 ? 0 : 11 - resto;

            // return cnpj.EndsWith(digito1.ToString() + digito2.ToString());

            return true;
        }

    }
}