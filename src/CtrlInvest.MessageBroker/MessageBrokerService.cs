using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Text;

namespace CtrlInvest.MessageBroker
{
    public class MessageBrokerService : IMessageBrokerService
    {
        private readonly DefaultObjectPool<IModel> _objectPool;
        private readonly ILogger _logger;        
        private readonly IModel _channel;
        private const string QueueName = "historical.price";
        private EventingBasicConsumer _consumer;
        public event EventHandler ProcessCompleted;

        public MessageBrokerService(IPooledObjectPolicy<IModel> objectPolicy, ILogger<MessageBrokerService> logger)
        {
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);

            _logger = logger;
            _channel = _objectPool.Get();
            _channel.QueueDeclare(
                    queue: QueueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
        }


        public void DoReceiveOperation()
        {
            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += (bc, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                try
                {
                    _logger.LogInformation($"message received : {message}");                    
                    ProcessCompleted?.Invoke(message, EventArgs.Empty);
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

        public void DoSendMessageOperation(string message)
        {
            throw new NotImplementedException();
        }

        public void SendMessageToRabbitMQ(string message)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(message);

                _channel.BasicPublish(exchange: "",
                                        routingKey: QueueName,
                                        basicProperties: null,
                                        body: body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _channel.Dispose();
                //_objectPool.Dispose();
            }
            // free native resources if there are any.
        }
    }
}
