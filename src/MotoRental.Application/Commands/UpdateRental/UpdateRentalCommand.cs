using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MotoRental.Core.Entities;

namespace MotoRental.Application.Commands.UpdateRental
{
    public class UpdateRentalCommand : IRequest
    {
        public UpdateRentalCommand(string rentalId, DateTime newDate)
        {
            RentalId = rentalId;
            NewDate = newDate;
        }
        
        public string RentalId { get; set; }
        public DateTime NewDate { get; set; }
    }
}