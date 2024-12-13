using System;
using System.Collections.Generic;
using MotoRental.Core.DTOs;
using MotoRental.Core.Exceptions;

namespace MotoRental.Core.Entities
{
    public class Rental : BaseEntity
    {
        private const decimal LateReturnFeePerDay = 50m;

        private Rental() { }

        public Rental(string motorcycleId, string deliveryPersonId, int planDays, DateTime startDate, DateTime endDate, DateTime expectedReturnDate)
        {
            MotorcycleId = motorcycleId;
            DeliveryPersonId = deliveryPersonId;
            PlanDays = planDays;
            StartDate = ValidateStartDate(startDate);
            EndDate = ValidateEndDate(endDate);
            ExpectedReturnDate = ValidateExpectedEndDate(expectedReturnDate);
            Penalty = 0m;
            TotalCost = CalculateInitialTotalCost();
            PlanDailyRate = GetOriginalPlanDailyRate();
        }

        public string MotorcycleId { get; private set; }
        public string DeliveryPersonId { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public DateTime ExpectedReturnDate { get; private set; }
        public DateTime? ActualReturnDate { get; private set; }
        public int PlanDays { get; private set; }
        public decimal PlanDailyRate { get; private set; }
        public decimal TotalCost { get; private set; }
        public decimal Penalty { get; private set; }

        private DateTime ValidateStartDate(DateTime startDate)
        {
            if (startDate.Date != DateTime.UtcNow.AddDays(1).Date)
            {
                throw new RentalBadRequestException($"A data de início deve ser {DateTime.UtcNow.AddDays(1):dd/MM/yyyy}");
            }

            return startDate.ToUniversalTime();
        }

        private DateTime ValidateEndDate(DateTime endDate)
        {
            if (endDate.Date != StartDate.AddDays(PlanDays).Date)
            {
                throw new RentalBadRequestException($"De acordo com o seu plano, a data de de entrega deve ser {StartDate.AddDays(PlanDays):dd/MM/yyyy}");
            }

            return endDate.ToUniversalTime();
        }

        private DateTime ValidateExpectedEndDate(DateTime expectedReturnDate)
        {
            if (expectedReturnDate.Date <= StartDate.Date)
            {
                throw new RentalBadRequestException("A data estimada final precisa ser maior que a data de início");
            }
            if (expectedReturnDate.Date > EndDate.Date)
            {
                throw new RentalBadRequestException("A data estimada final não pode ser maior que a data final");
            }

            return expectedReturnDate.ToUniversalTime();
        }

        private decimal GetOriginalPlanDailyRate()
        {
            return PlanDays switch
            {
                PlanTypes.SevenDays => 30m,
                PlanTypes.FifteenDays => 28,
                PlanTypes.ThirtyDays => 22,
                PlanTypes.FortyFiveDays => 20,
                PlanTypes.FiftyDays => 18,
                _ => 0m,
            };
        }
        private decimal CalculatePenalty()
        {
            if (ActualReturnDate is null)
                return 0m;

            var finalDate = ActualReturnDate.Value;
            var planDailyRate = GetOriginalPlanDailyRate();

            if (finalDate < ExpectedReturnDate)
            {
                var unusedDays = (EndDate - finalDate).Days;
                var unusedCost = unusedDays * planDailyRate;

                return PlanDays switch
                {
                    PlanTypes.SevenDays => unusedCost * 0.2m,
                    PlanTypes.FifteenDays => unusedCost * 0.4m,
                    _ => 0m,
                };
            }
            
            if (finalDate > ExpectedReturnDate)
            {
                var extraDays = (finalDate - EndDate).Days;
                return extraDays * LateReturnFeePerDay;
            }

            return 0m;
        }

        private decimal CalculateInitialTotalCost()
        {
            var originalPlanDailyRate = GetOriginalPlanDailyRate();

            return PlanDays * originalPlanDailyRate;
        }
        private decimal CalculateFinalTotalCost()
        {
            if (ActualReturnDate is null)
                return 0m;

            var finalDate = ActualReturnDate.Value;
            var originalPlanDailyRate = GetOriginalPlanDailyRate();

            if (finalDate < ExpectedReturnDate)
            {
                var usedDays = (finalDate - StartDate).Days;
                return (usedDays * originalPlanDailyRate) + Penalty;
            }
            
            if (finalDate > ExpectedReturnDate)
            {
                return (PlanDays * originalPlanDailyRate) + Penalty;
            }

            return PlanDays * originalPlanDailyRate;
        }


        public void SetReturnDate(DateTime actualReturnDate)
        {
            if (actualReturnDate.Date <= StartDate.Date)
            {
                throw new RentalBadRequestException("A data de retorno precisa ser maior que a data de início");
            }

            ActualReturnDate = actualReturnDate.ToUniversalTime();
            Penalty = CalculatePenalty();
            TotalCost = CalculateFinalTotalCost();
        }

        public static bool IsValidPlan(int plan)
        {
            if (plan != PlanTypes.SevenDays && 
                plan != PlanTypes.FifteenDays && 
                plan != PlanTypes.ThirtyDays &&
                plan != PlanTypes.FortyFiveDays &&
                plan != PlanTypes.FiftyDays)
            {
                return false;
            }

            return true;
        }
    }

    public static class PlanTypes
    {
        public const int SevenDays = 7;
        public const int FifteenDays  = 15;
        public const int ThirtyDays = 30;
        public const int FortyFiveDays  = 45;
        public const int FiftyDays  = 50;
    }

}
