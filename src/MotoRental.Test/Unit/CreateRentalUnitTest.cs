using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MotoRental.Application.Commands.CreateRental;
using MotoRental.Core.Entities;
using MotoRental.Core.Exceptions;
using Xunit;

namespace MotoRental.Test.Unit
{
    public class CreateRentalUnitTest
    {
        private CreateRentalCommand command = new CreateRentalCommand 
        {
            moto_id = "dummy moto id",
            entregador_id = "dummy deliveryPerson id",
            plano = PlanTypes.SevenDays,
            data_inicio = DateTime.Today.AddDays(1),
            data_termino = DateTime.Today.AddDays(PlanTypes.SevenDays + 1),
            data_previsao_termino = DateTime.Today.AddDays(PlanTypes.SevenDays + 1)
        };
        [Fact]
        public void CreateRentalCommand_ToEntity_OnSucces()
        {
            var rental = CreateRentalCommand.ToEntity(command);

            Assert.NotNull(rental);
            Assert.Equal(command.moto_id, rental.MotorcycleId);
            Assert.Equal(command.entregador_id, rental.DeliveryPersonId);
            Assert.Equal(command.plano, rental.PlanDays);
            Assert.Equal(command.data_inicio.ToUniversalTime(), rental.StartDate);
            Assert.Equal(command.data_termino.ToUniversalTime(), rental.EndDate);
            Assert.Equal(command.data_previsao_termino.ToUniversalTime(), rental.ExpectedReturnDate);  
        }
        [Fact]
        public void CreateRentalCommand_ToEntity_OnSucces_Values_WhenReturnAfterEndDate()
        {
            var rental = CreateRentalCommand.ToEntity(command);

            var totalCost = rental.PlanDailyRate * rental.PlanDays;

            Assert.Equal(totalCost, rental.TotalCost);

            rental.SetReturnDate(rental.ExpectedReturnDate.AddDays(1));

            var penalty = 50;
    
            Assert.Equal(penalty, rental.Penalty);
            Assert.Equal(totalCost + penalty, rental.TotalCost);
        }
        [Fact]
        public void CreateRentalCommand_ToEntity_OnSucces_Values_WhenReturnBeforeEndDate()
        {
            var rental = CreateRentalCommand.ToEntity(command);

            var totalCost = rental.PlanDailyRate * rental.PlanDays;
            var originalPlanDailyRate = rental.PlanDailyRate;

            Assert.Equal(totalCost, rental.TotalCost);

            var unusedDays = 1;
            var unusedCost = rental.PlanDailyRate * unusedDays;

            rental.SetReturnDate(rental.ExpectedReturnDate.AddDays(-unusedDays));

            var penalty = rental.PlanDays switch
            {
                PlanTypes.SevenDays => unusedCost * 0.2m,
                PlanTypes.FifteenDays => unusedCost * 0.4m,
                _ => 0m,
            };

            var newTotalCost = (rental.PlanDays - unusedDays) * originalPlanDailyRate;
    
            Assert.Equal(penalty, rental.Penalty);
            Assert.Equal(newTotalCost + penalty, rental.TotalCost);
        }
        [Fact]
        public void CreateRentalCommand_ToEntity_OnFailure_WrongInitialDate()
        {
            command.data_inicio = DateTime.Today;

            Assert.Throws<RentalBadRequestException>(() => CreateRentalCommand.ToEntity(command));
        }
        [Fact]
        public void CreateRentalCommand_ToEntity_OnFailure_WrongEndDate()
        {
            command.data_termino = DateTime.Today;

            Assert.Throws<RentalBadRequestException>(() => CreateRentalCommand.ToEntity(command));
        }
        [Fact]
        public void CreateRentalCommand_ToEntity_OnFailure_WrongExpectedEndDate()
        {
            command.data_previsao_termino = DateTime.Today;

            Assert.Throws<RentalBadRequestException>(() => CreateRentalCommand.ToEntity(command));
        }

    }
}