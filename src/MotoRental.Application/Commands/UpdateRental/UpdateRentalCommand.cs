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
        public UpdateRentalCommand(int rentalId, DateTime newDate)
        {
            RentalId = rentalId;
            NewDate = newDate;
        }
        
        public int RentalId { get; set; }
        public DateTime NewDate { get; set; }
    }
}