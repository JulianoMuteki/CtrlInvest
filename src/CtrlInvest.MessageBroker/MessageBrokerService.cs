using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CtrlInvest.MessageBroker
{
    public class MessageBrokerService : IMessageBrokerService
    {
        private readonly DefaultObjectPool<IModel> _objectPool;
        private readonly ILogger _logger;        
        private readonly IModel _channel;
        private AsyncEventingBasicConsumer _consumer;
        public event EventHandler<string> ProcessCompleted;

        private string _queueName;

        public MessageBrokerService(IPooledObjectPolicy<IModel> objectPolicy, ILogger<MessageBrokerService> logger)
        {
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);

            _logger = logger;
            _channel = _objectPool.Get();
        }

        private void SetQueueName(string queueName)
        {
            _queueName = queueName;
            _channel.QueueDeclare(
                queue: _queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }
        public void SetQueueChannel(string queueName)
        {
            SetQueueName(queueName);
            
        }

        public async Task DoReceiveMessageOperation(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            _consumer = new AsyncEventingBasicConsumer(_channel);
            _consumer.Received += async (bc, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                _logger.LogInformation($"Processing msg: '{message}'.");
                try
                {
                    
                    _logger.LogInformation($"message received : {message} ********** {_queueName}");
                     ProcessCompleted?.Invoke(message, _queueName);              
                    _channel.BasicAck(ea.DeliveryTag, false);
                    await Task.CompletedTask;
                }
                catch (JsonException)
                {
                    _logger.LogError($"JSON Parse Error: '{message}'.");
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                }
                catch (AlreadyClosedException)
                {
                    _logger.LogInformation("RabbitMQ is closed!");
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                }
                catch (Exception e)
                {
                    _logger.LogError(default, e, e.Message);
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                }
            };

            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: _consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Receiving running {Queue} at: {time}", _queueName, DateTimeOffset.Now);
                await Task.Delay(1000 * 100, stoppingToken);
            }

            await Task.CompletedTask;
        }


        public void DoSendMessageOperation(string message)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(message);

                _channel.BasicPublish(exchange: "",
                                        routingKey: _queueName,
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
