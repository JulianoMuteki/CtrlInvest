using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Import.HistoricalPrice
{
    public class Broker
    {
        public const string QueueName = "historical.price";
        public void SendMessageToRabbitMQ(string historicalPriceList)
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
            };

            using var sr = new StringReader(historicalPriceList);
            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {   

                    channel.QueueDeclare(
                        queue: QueueName,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var body = Encoding.UTF8.GetBytes(line);

                    channel.BasicPublish(exchange: "",
                                         routingKey: QueueName,
                                         basicProperties: null,
                                         body: body);


                }
            }
        }
    }
}
