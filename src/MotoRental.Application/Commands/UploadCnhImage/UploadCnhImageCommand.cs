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
        public string deliveryPersonId { get; set; }
        public string imagem_cnh { get; set; }
    }
}