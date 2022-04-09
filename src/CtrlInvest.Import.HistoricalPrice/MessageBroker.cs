using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Text;

namespace CtrlInvest.Import.HistoricalPrice
{
    public class MessageBroker: IMessageBroker
    {
        private readonly ILogger<MessageBroker> _logger;

        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        private const string QueueName = "historical.price";
        public MessageBroker(ILogger<MessageBroker> logger)
        {
            _logger = logger;
        }

        public void Init()
        {
            _connectionFactory = new ConnectionFactory()
            {
                //HostName = "172.18.0.0", // Heroku
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
                _connection.Dispose();
            }
            // free native resources if there are any.
        }
    }
}
