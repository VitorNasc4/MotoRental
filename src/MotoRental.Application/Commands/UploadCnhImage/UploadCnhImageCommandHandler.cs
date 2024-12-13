using System.Threading;
using System.Threading.Tasks;
using MotoRental.Core.Repositories;
using MediatR;
using MotoRental.Core.Exceptions;
using MotoRental.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MotoRental.Application.Commands.UploadCnhImage
{
    public class UploadCnhImageCommandHandler : IRequestHandler<UploadCnhImageCommand, Unit>
    {
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;
        private readonly IImageUploadService _imageUploadService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UploadCnhImageCommandHandler> _logger;

        public UploadCnhImageCommandHandler(
            IDeliveryPersonRepository deliveryPersonRepository, 
            IImageUploadService imageUploadService, 
            IConfiguration configuration,
            ILogger<UploadCnhImageCommandHandler> logger)
        {
            _deliveryPersonRepository = deliveryPersonRepository;
            _imageUploadService = imageUploadService;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<Unit> Handle(UploadCnhImageCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Iniciando processo de upload da imagem da CNH. Id do entregador: {request.DeliveryPersonId}");
            var deliveryPerson = await _deliveryPersonRepository.GetDeliveryPersonByIdAsync(request.DeliveryPersonId);
            
            if (deliveryPerson is null)
            {
                _logger.LogError($"Interrompendo processo de upload da imagem da CNH. Motivo: Entregador não encontrado. Id buscado: {request.DeliveryPersonId}");
                throw new DeliveryPersonNotFoundException(request.DeliveryPersonId);
            }

            var containerName = _configuration["AzureBlobService:ContainerName"];

            _logger.LogInformation($"Enviando imagem para serviço de imagem...");
            var imageUrl = await _imageUploadService.UploadBase64Image(request.ImagemCnh, containerName);

            deliveryPerson.SetCNH_Image(imageUrl);

            await _deliveryPersonRepository.SaveChangesAsync();

            _logger.LogInformation($"Finalizando processo de upload da imagem da CNH. Id do registro atualizado: {deliveryPerson.Id}");

            return Unit.Value;
        }
    }
}