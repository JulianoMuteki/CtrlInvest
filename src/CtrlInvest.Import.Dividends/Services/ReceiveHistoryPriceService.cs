using CtrlInvest.MessageBroker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Text;


namespace CtrlInvest.Import.Dividends.Services
{
    //public class ReceiveHistoryPriceService : MessageBrokerService, IReceiveHistoryPriceService
    //{
    //    private int executionCount = 0;
    //    private readonly ILogger _logger;

    //    private IModel _channel;
    //    private const string QueueName = "historical.price";
    //    private EventingBasicConsumer _consumer;

    //    public ReceiveHistoryPriceService(IPooledObjectPolicy<IModel> objectPolicy, ILogger<ReceiveHistoryPriceService> logger)
    //        :base(objectPolicy)
    //    {
    //        _logger = logger;

    //        _channel = this.GetChannel();
    //        _channel.QueueDeclare(
    //                queue: QueueName,
    //                durable: false,
    //                exclusive: false,
    //                autoDelete: false,
    //                arguments: null);

    //        _consumer = new EventingBasicConsumer(_channel);
    //    }
    //    public override void DoOperation()
    //    {
    //        _consumer.Received += (bc, ea) =>
    //        {
    //            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
    //            try
    //            {
    //                _logger.LogInformation($"message received : {message}");
    //               // _historicalPriceService1.SaveInDatabaseOperation(message);
    //            }
    //            catch (JsonException)
    //            {
    //                _logger.LogError($"JSON Parse Error: '{message}'.");
    //                _channel.BasicNack(ea.DeliveryTag, false, false);
    //            }
    //            catch (AlreadyClosedException)
    //            {
    //                _logger.LogInformation("RabbitMQ is closed!");
    //            }
    //            catch (Exception e)
    //            {
    //                _logger.LogError(default, e, e.Message);
    //            }
    //        };

    //        _channel.BasicConsume(queue: QueueName,
    //           autoAck: true,
    //           consumer: _consumer);
    //    }
    //    //public async Task DoService(CancellationToken stoppingToken, string key)
    //    //{
    //    //    while (!stoppingToken.IsCancellationRequested)
    //    //    {
    //    //        executionCount++;

    //    //        _logger.LogInformation(
    //    //            "Scoped Processing Service is working. Count: {Count} - key: {key}", executionCount, key);

    //    //        await Task.Delay(1000, stoppingToken);
    //    //    }
    //    //}
    //}
}
