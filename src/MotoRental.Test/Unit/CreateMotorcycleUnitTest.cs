using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MotoRental.Application.Commands.CreateMotorcycle;
using MotoRental.Core.Exceptions;
using MotoRental.Core.Repositories;
using MotoRental.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class CreateMotorcycleUnitTest
{
    private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;
    private readonly Mock<IMessageBusService> _messageBusServiceMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<ILogger<CreateMotorcycleCommandHandler>> _loggerMock;
    private readonly CreateMotorcycleCommandHandler _handler;

    public CreateMotorcycleUnitTest()
    {
        _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();
        _messageBusServiceMock = new Mock<IMessageBusService>();
        _configurationMock = new Mock<IConfiguration>();
        _loggerMock = new Mock<ILogger<CreateMotorcycleCommandHandler>>();
        _handler = new CreateMotorcycleCommandHandler(
            _motorcycleRepositoryMock.Object,
            _messageBusServiceMock.Object,
            _configurationMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenMotorcyclePlateAlreadyExists()
    {
        var command = new CreateMotorcycleCommand { placa = "XYZ1234" };
        _motorcycleRepositoryMock
            .Setup(repo => repo.MotorcyclePlateAlreadyExistsAsync(command.placa))
            .ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<MotorcycleAlreadyExistsException>(() => _handler.Handle(command, CancellationToken.None));

        _motorcycleRepositoryMock.Verify(repo => repo.MotorcyclePlateAlreadyExistsAsync(command.placa), Times.Once);
        _messageBusServiceMock.Verify(bus => bus.Publish(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldPublishMessage_WhenMotorcyclePlateDoesNotExist()
    {
        var command = new CreateMotorcycleCommand { placa = "XYZ1234", modelo = "Honda", ano = "2024" };
        _motorcycleRepositoryMock
            .Setup(repo => repo.MotorcyclePlateAlreadyExistsAsync(command.placa))
            .ReturnsAsync(false);
        _configurationMock
            .Setup(config => config["RabbitmqConfig:QueueName"])
            .Returns("MotorcycleQueue");

        var expectedJson = JsonSerializer.Serialize(CreateMotorcycleCommand.ToDTO(command));
        var expectedBytes = Encoding.UTF8.GetBytes(expectedJson);

        await _handler.Handle(command, CancellationToken.None);

        _messageBusServiceMock.Verify(bus => bus.Publish("MotorcycleQueue", expectedBytes), Times.Once);
        _motorcycleRepositoryMock.Verify(repo => repo.MotorcyclePlateAlreadyExistsAsync(command.placa), Times.Once);
    }
}
