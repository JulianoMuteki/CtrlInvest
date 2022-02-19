
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;

namespace CtrlInvest.MessageBroker
{
    public class RabbitFactory : IRabbitFactoryConnection
    {
        private readonly RabbitOptions _options;

        private readonly IConnection _connection;

        public RabbitFactory(IOptions<RabbitOptions> optionsAccs)            
        {
            _options = optionsAccs.Value;
           
            _connection = GetConnection();
            _connection.ConnectionShutdown += Connection_ConnectionShutdown;
        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _options.HostName,
                UserName = _options.UserName,
                Password = _options.Password,
                Port = _options.Port,
                VirtualHost = _options.VHost,
                //  AutomaticRecoveryEnabled = true,
                //  RequestedChannelMax = 4,
                //   NetworkRecoveryInterval = System.TimeSpan.FromSeconds(10),
                DispatchConsumersAsync = true
            };

            return factory.CreateConnection();
        }

        private static void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {

        }

        public IModel Create()
        {
            return _connection.CreateModel();
        }

        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
            {
                return true;
            }
            else
            {
                obj?.Dispose();
                this.Dispose(true);
                return false;
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
                if (_connection.IsOpen)
                    _connection.Close();

                _connection.Dispose();

            }
        }
    }
}
