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
        private EventingBasicConsumer _consumer;
        private string _queueName;
        private DateTime _expireTime = DateTime.Now;

        public event EventHandler<string> ProcessCompleted;
        public MessageBrokerService(IRabbitFactoryConnection objectPolicy, ILogger<MessageBrokerService> logger)
        {
            _logger = logger;
            _logger.LogInformation("************ Start Message Broker ************");
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);          
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

        public void DoReceiveMessageOperation()
        {
            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += (bc, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                try
                {
                    _logger.LogTrace($"Processing msg: '{message}' at {DateTime.Now}");
                    _expireTime = DateTime.Now;
                    _channel.BasicAck(ea.DeliveryTag, false);
                    ProcessCompleted?.Invoke(message, _queueName);
                }
                catch (JsonException je)
                {
                    _logger.LogError($"JSON Parse Error: '{message}'.");
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                    throw je;
                }
                catch (AlreadyClosedException z)
                {
                    _logger.LogInformation("RabbitMQ is closed!");
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                    throw z;
                }
                catch (Exception e)
                {
                    _logger.LogError(default, e, e.Message);
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                    throw e;
                }
            };

            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: _consumer);
        }

        public bool isExpireTimeToReceiveMessage()
        {
            if (DateTime.Now > _expireTime.AddMinutes(15))
                return true;
            else
                return false;
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
                //TODO: Check close consumer
                if (_channel.IsOpen)
                    _channel.Close();

                _channel.Dispose();
            }
        }

        public bool ChannelConnectionIsOpen()
        {
            return _channel.IsOpen;
        }
    }
}
