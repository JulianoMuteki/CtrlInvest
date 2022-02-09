using CtrlInvest.MessageBroker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Text;

namespace CtrlInvest.Receive.HistoricalData.Services
{
    public class ReceiveHistoryPriceService : MessageBrokerService, IReceiveHistoryPriceService
    {
        private int executionCount = 0;
        private readonly ILogger _logger;
        private readonly IHistoricalPriceService _historicalPriceService1;
        private readonly IModel _channel;

        private const string QueueName = "historical.price";
        private EventingBasicConsumer _consumer;

        public ReceiveHistoryPriceService(IPooledObjectPolicy<IModel> objectPolicy, ILogger<ReceiveHistoryPriceService> logger, 
                                            IHistoricalPriceService historicalPriceService)
            : base(objectPolicy)
        {
            _logger = logger;
            _historicalPriceService1 = historicalPriceService;
            _channel = this.GetChannel();
            _channel.QueueDeclare(
                    queue: QueueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

            
        }
        public override void DoOperation()
        {
            _consumer = new EventingBasicConsumer(_channel);
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
