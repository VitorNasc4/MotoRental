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
        private readonly IConfiguration _configuration;
        private readonly ConnectionFactory _factory;
        private readonly ILogger<MotorcycleConsumer> _logger;
        private IConnection CreateRabbitMqConnectionWithRetry(ConnectionFactory factory)
        {
            int maxRetries = 10;
            int retryDelay = 5000; 

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    Console.WriteLine($"Tentativa {attempt} de conexão com RabbitMQ...");
                    return factory.CreateConnection();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Falha ao conectar ao RabbitMQ: {ex.Message}");

                    if (attempt == maxRetries)
                    {
                        Console.WriteLine("Número máximo de tentativas atingido. Abortando...");
                        throw; 
                    }

                    Thread.Sleep(retryDelay);
                }
            }

            throw new Exception("Não foi possível conectar ao RabbitMQ após várias tentativas.");
        }

        public MotorcycleConsumer(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<MotorcycleConsumer> logger)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _logger = logger;

            var RABBITMQ_HOST = Environment.GetEnvironmentVariable("RABBITMQ_HOST");

            if (RABBITMQ_HOST is null)
            {
                _factory = new ConnectionFactory
                {
                    HostName = "localhost"
                };
            }
            else
            {
                _factory = new ConnectionFactory 
                {
                    HostName = RABBITMQ_HOST, 
                    UserName = "guest",
                    Password = "guest",
                    Port = 5672
                };
            }

            _connection = CreateRabbitMqConnectionWithRetry(_factory);
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: _configuration["RabbitmqConfig:QueueName"],
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
                var byteArray = eventArgs.Body.ToArray();
                var motorcycleInfoJson = Encoding.UTF8.GetString(byteArray);
                var motorcycleInfoDTO = JsonSerializer.Deserialize<MotorcycleInfoDTO>(motorcycleInfoJson);

                if (motorcycleInfoDTO is not null)
                {
                  _logger.LogError($"IMensagem recebida: {motorcycleInfoDTO}");
                  var motorcycle = MotorcycleInfoDTO.ToEntity(motorcycleInfoDTO);

                  using (var scope = _serviceProvider.CreateScope())
                  {
                      var dbContext = scope.ServiceProvider.GetRequiredService<MotoRentalDbContext>();
                      try
                      {
                          await dbContext.Motorcycles.AddAsync(motorcycle);
                          await dbContext.SaveChangesAsync();

                          _logger.LogTrace($"Registro criado com sucesso. Id: {motorcycle.Id}");

                          if (motorcycle.Year == "2024")
                            _logger.LogTrace($"A moto registrada é do ano 2024");

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

            _channel.BasicConsume(_configuration["RabbitmqConfig:QueueName"], false, consumer);

            return Task.CompletedTask;
        }
    }
}