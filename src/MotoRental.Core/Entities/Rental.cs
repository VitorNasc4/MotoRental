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

        public Rental(int motorcycleId, int deliveryPersonId, int planDays, DateTime startDate, DateTime endDate, DateTime expectedReturnDate)
        {
            MotorcycleId = motorcycleId;
            DeliveryPersonId = deliveryPersonId;
            PlanDays = planDays;
            StartDate = ValidateStartDate(startDate);
            EndDate = ValidateEndDate(endDate);
            ExpectedReturnDate = ValidateExpectedEndDate(expectedReturnDate);
            Penalty = CalculatePenalty();
            TotalCost = CalculateTotalCost();
            PlanDailyRate = CalculatePlanDailyRate();
        }

        public int MotorcycleId { get; private set; }
        public int DeliveryPersonId { get; private set; }
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
                throw new RentalBadRequestException($"A data de início deve ser {DateTime.UtcNow.AddDays(1):dd/MM/yyyy HH:mm:ss}");
            }

            return startDate.ToUniversalTime();
        }

        private DateTime ValidateEndDate(DateTime endDate)
        {
            if (endDate.Date != StartDate.AddDays(PlanDays).Date)
            {
                throw new RentalBadRequestException($"De acordo com o seu plano, a data de de entrega deve ser {StartDate.AddDays(PlanDays):dd/MM/yyyy HH:mm:ss}");
            }

            return endDate.ToUniversalTime();
        }

        private DateTime ValidateExpectedEndDate(DateTime expectedReturnDate)
        {
            if (expectedReturnDate.Date < StartDate.Date)
            {
                throw new RentalBadRequestException("A data estimada final precisa ser maior que a data de início");
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
            var finalDate = ActualReturnDate ?? ExpectedReturnDate;
            var planDailyRate = GetOriginalPlanDailyRate();

            if (finalDate < EndDate)
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
            
            if (finalDate > EndDate)
            {
                var extraDays = (finalDate - EndDate).Days;
                return extraDays * LateReturnFeePerDay;
            }

            return 0m;
        }

        private decimal CalculateTotalCost()
        {
            var finalDate = ActualReturnDate ?? ExpectedReturnDate;
            var originalPlanDailyRate = GetOriginalPlanDailyRate();

            if (finalDate < EndDate)
            {
                var usedDays = (finalDate - StartDate).Days;
                return (usedDays * originalPlanDailyRate) + Penalty;
            }
            
            if (finalDate > EndDate)
            {
                return (PlanDays * originalPlanDailyRate) + Penalty;
            }

            return PlanDays * originalPlanDailyRate;
        }

        private decimal CalculatePlanDailyRate()
        {
            var finalDate = ActualReturnDate ?? ExpectedReturnDate;
            
            var usedDays = (finalDate - StartDate).Days;
            
            return TotalCost / usedDays;
        
        }

        public void SetReturnDate(DateTime actualReturnDate)
        {
            if (actualReturnDate.Date < StartDate.Date)
            {
                throw new RentalBadRequestException("A data de retorno precisa ser maior que a data de início");
            }

            ActualReturnDate = actualReturnDate.ToUniversalTime();
            Penalty = CalculatePenalty();
            TotalCost = CalculateTotalCost();
            PlanDailyRate = CalculatePlanDailyRate();
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
