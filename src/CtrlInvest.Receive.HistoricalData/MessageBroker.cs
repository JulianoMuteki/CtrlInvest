using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CtrlInvest.Receive.HistoricalData
{
    public class MessageBroker : IMessageBroker
    {
        private readonly ILogger<Worker> _logger;
        private IHistoricalPriceService _historicalPriceService1;

        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        private const string QueueName = "historical.price";
        private EventingBasicConsumer _consumer;
        public MessageBroker(ILogger<Worker> logger, IHistoricalPriceService historicalPriceService)
        {
            _logger = logger;
            _historicalPriceService1 = historicalPriceService;

            _connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
            };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                       queue: QueueName,
                       durable: false,
                       exclusive: false,
                       autoDelete: false,
                       arguments: null);

            _consumer = new EventingBasicConsumer(_channel);
        }

        public void DoReceiveMessageOperation()
        {
            _consumer.Received += (bc, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                try
                {
                    _logger.LogInformation($"message received : {message}");
                    _historicalPriceService1.SaveInDatabaseOperation(message);
                }
                catch (JsonException)
                {
                    _logger.LogError($"JSON Parse Error: '{message}'.");
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                }
                catch (AlreadyClosedException)
                {
                    _logger.LogInformation("RabbitMQ is closed!");
                }
                catch (Exception e)
                {
                    _logger.LogError(default, e, e.Message);
                }
            };

            _channel.BasicConsume(queue: QueueName,
               autoAck: true,
               consumer: _consumer);
        }
    }
}
