using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MotoRental.Core.Services;
using RabbitMQ.Client;

namespace MotoRental.Infrastructure.MessageBus
{
    public class MessageBusService : IMessageBusService
    {
        private readonly ConnectionFactory _factory;
        public MessageBusService()
        {
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

        }
        public void Publish(string queue, byte[] message)
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    // Garantir que a fila esteja criada
                    channel.QueueDeclare(
                        queue: queue,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    // Publicar mensagem
                    channel.BasicPublish(
                        exchange: "",
                        routingKey: queue,
                        basicProperties: null,
                        body: message
                    );
                }
            }
        }
    }
}