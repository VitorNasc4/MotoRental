using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MotoRental.Core.Entities;

namespace MotoRental.Application.Commands.UploadCnhImage
{
    public class UploadCnhImageCommand : IRequest<Unit>
    {
        public string DeliveryPersonId { get; set; }
        public string ImagemCnh { get; set; }
        public UploadCnhImageCommand(string deliveryPersonId, string imageCnh)
        {
            DeliveryPersonId = deliveryPersonId;
            ImagemCnh = imageCnh;
        }
    }
}