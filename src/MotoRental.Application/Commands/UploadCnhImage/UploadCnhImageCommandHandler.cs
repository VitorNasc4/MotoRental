using System.Threading;
using System.Threading.Tasks;
using MotoRental.Core.Repositories;
using MediatR;
using MotoRental.Core.Exceptions;
using MotoRental.Core.Services;
using Microsoft.Extensions.Configuration;

namespace MotoRental.Application.Commands.UploadCnhImage
{
    public class UploadCnhImageCommandHandler : IRequestHandler<UploadCnhImageCommand, Unit>
    {
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;
        private readonly IImageUploadService _imageUploadService;
        private readonly IConfiguration _configuration;

        public UploadCnhImageCommandHandler(IDeliveryPersonRepository deliveryPersonRepository, IImageUploadService imageUploadService, IConfiguration configuration)
        {
            _deliveryPersonRepository = deliveryPersonRepository;
            _imageUploadService = imageUploadService;
            _configuration = configuration;
        }
        public async Task<Unit> Handle(UploadCnhImageCommand request, CancellationToken cancellationToken)
        {
            var deliveryPerson = await _deliveryPersonRepository.GetDeliveryPersonByIdAsync(request.DeliveryPersonId);
            
            if (deliveryPerson is null)
            {
                throw new DeliveryPersonNotFoundException(request.DeliveryPersonId);
            }

            var containerName = _configuration["AzureBlobService:ContainerName"];

            var imageUrl = await _imageUploadService.UploadBase64Image(request.ImagemCnh, containerName);

            deliveryPerson.SetCNH_Image(imageUrl);

            await _deliveryPersonRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}