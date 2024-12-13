using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using MotoRental.Application.Commands.UploadCnhImage;
using MotoRental.Core.Repositories;
using MotoRental.Core.Exceptions;
using MotoRental.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MotoRental.Core.Entities;

namespace MotoRental.Test.Unit
{
    public class UploadCnhImageUnitTest
    {
        private readonly Mock<IDeliveryPersonRepository> _deliveryPersonRepositoryMock;
        private readonly Mock<IImageUploadService> _imageUploadServiceMock;
        private readonly Mock<ILogger<UploadCnhImageCommandHandler>> _loggerMock;
        private readonly Mock<IConfiguration> _configurationMock;

        private readonly UploadCnhImageCommandHandler _handler;

        public UploadCnhImageUnitTest()
        {
            _deliveryPersonRepositoryMock = new Mock<IDeliveryPersonRepository>();
            _imageUploadServiceMock = new Mock<IImageUploadService>();
            _loggerMock = new Mock<ILogger<UploadCnhImageCommandHandler>>();
            _configurationMock = new Mock<IConfiguration>();

            _configurationMock
                .Setup(c => c["AzureBlobService:ContainerName"])
                .Returns("test-container");

            _handler = new UploadCnhImageCommandHandler(
                _deliveryPersonRepositoryMock.Object,
                _imageUploadServiceMock.Object,
                _configurationMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldUploadImageAndSaveToRepository_OnSuccess()
        {
            var command = new UploadCnhImageCommand(Guid.NewGuid().ToString(), "dummyBase64Image");

            var deliveryPerson = new DeliveryPerson("Test Name", "12345678901234", DateTime.Now, "123456", "A", "");

            _deliveryPersonRepositoryMock
                .Setup(repo => repo.GetDeliveryPersonByIdAsync(command.DeliveryPersonId))
                .ReturnsAsync(deliveryPerson);

            _imageUploadServiceMock
                .Setup(service => service.UploadBase64Image(command.ImagemCnh, "test-container"))
                .ReturnsAsync("https://example.com/image.jpg");

            _deliveryPersonRepositoryMock
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            await _handler.Handle(command, CancellationToken.None);

            _imageUploadServiceMock.Verify(service =>
                service.UploadBase64Image(command.ImagemCnh, "test-container"), Times.Once);

            _deliveryPersonRepositoryMock.Verify(repo =>
                repo.SaveChangesAsync(), Times.Once);

            Assert.Equal("https://example.com/image.jpg", deliveryPerson.CNH_Image);
        }

        [Fact]
        public async Task Handle_ShouldThrowDeliveryPersonNotFoundException_WhenDeliveryPersonNotFound()
        {

            var command = new UploadCnhImageCommand(Guid.NewGuid().ToString(), "dummyBase64Image");

            _deliveryPersonRepositoryMock
                .Setup(repo => repo.GetDeliveryPersonByIdAsync(command.DeliveryPersonId))
                .ReturnsAsync((DeliveryPerson)null!);

    
            await Assert.ThrowsAsync<DeliveryPersonNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None));

            _imageUploadServiceMock.Verify(service =>
                service.UploadBase64Image(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

            _deliveryPersonRepositoryMock.Verify(repo =>
                repo.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenExceptionOccurs()
        {

            var command = new UploadCnhImageCommand(Guid.NewGuid().ToString(), "dummyBase64Image");

            var deliveryPerson = new DeliveryPerson("Test Name", "12345678901234", DateTime.Now, "123456", "A", "");

            _deliveryPersonRepositoryMock
                .Setup(repo => repo.GetDeliveryPersonByIdAsync(command.DeliveryPersonId))
                .ReturnsAsync(deliveryPerson);

            _imageUploadServiceMock
                .Setup(service => service.UploadBase64Image(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Upload failed"));

            await Assert.ThrowsAsync<Exception>(
                () => _handler.Handle(command, CancellationToken.None));

            _deliveryPersonRepositoryMock.Verify(repo =>
                repo.SaveChangesAsync(), Times.Never);
        }
    }
}
