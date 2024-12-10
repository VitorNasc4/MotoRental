using System.Text;
using System.Text.Json;
using MotoRental.Core.DTOs;
using MotoRental.Infrastructure.Persistence;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MotoRental.Messaging.Consumer
{
    public class MotorcycleConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;
        private const string QUEUE = "motorcycle-events";

        public MotorcycleConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: QUEUE,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (sender, eventArgs) =>
            {
                Console.WriteLine("CHEGOOUUU");
                var byteArray = eventArgs.Body.ToArray();
                var motorcycleInfoJson = Encoding.UTF8.GetString(byteArray);
                var motorcycleInfoDTO = JsonSerializer.Deserialize<MotorcycleInfoDTO>(motorcycleInfoJson);

                if (motorcycleInfoDTO is not null)
                {
                  Console.WriteLine($"Mensagem recebida: {motorcycleInfoDTO}");
                  var motorcycle = MotorcycleInfoDTO.ToEntity(motorcycleInfoDTO);

                  using (var scope = _serviceProvider.CreateScope())
                  {
                      var dbContext = scope.ServiceProvider.GetRequiredService<MotoRentalDbContext>();
                      try
                      {
                          await dbContext.Motorcycles.AddAsync(motorcycle);
                          await dbContext.SaveChangesAsync();
                          Console.WriteLine("Moto salva com sucesso!");

                          if (motorcycle.Year == "2024")
                            Console.WriteLine("Moto do ano 2024 registrada!");

                      }
                      catch (Exception e)
                      {
                          Console.WriteLine("Erro ao salvar a moto");
                          Console.WriteLine(e);
                      }
                  }
                }

                _channel.BasicAck(eventArgs.DeliveryTag, false);

            };

            _channel.BasicConsume(QUEUE, false, consumer);

            return Task.CompletedTask;
        }
    }
}