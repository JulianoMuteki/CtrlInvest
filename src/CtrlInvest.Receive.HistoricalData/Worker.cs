using CtrlInvest.MessageBroker;
using CtrlInvest.MessageBroker.Helpers;
using CtrlInvest.Services.StocksExchanges;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CtrlInvest.Receive.HistoricalData
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHistoricalPriceService _historicalPriceService;
        private readonly IHistoricalEarningService _historicalEarningService;
        IMessageBrokerService messageBrokerService = null;

        public Worker(IServiceProvider services, ILogger<Worker> logger, IHistoricalPriceService historicalPriceService, IHistoricalEarningService historicalEarningService)
        {
            _historicalPriceService = historicalPriceService;
            _historicalEarningService = historicalEarningService;
            _serviceProvider = services;
            _logger = logger;      
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Worker started at: {DateTime.Now}");
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                stoppingToken.ThrowIfCancellationRequested();

                await Task.Run(async () =>
                {
                    await StartProcessReceiveMessage(stoppingToken, QueueName.HISTORICAL_PRICE);
                }, stoppingToken);

                //Tempo para rodar Broker novamente
                //5 horas
                await Task.Delay(60000 * 5, stoppingToken);
            }

            await Task.CompletedTask;
        }

        private Task StartProcessReceiveMessage(CancellationToken stoppingToken, string queueName)
        {           
            try
            {
                using var scope = _serviceProvider.CreateScope();

                messageBrokerService = scope.ServiceProvider
                                                .GetService<IMessageBrokerService>();

                messageBrokerService.ProcessCompleted += EventHandler_MessageReceived;
                messageBrokerService.SetQueueChannel(queueName);
                messageBrokerService.DoReceiveMessageOperation();

                while (messageBrokerService.channelIsOpen())
                {
                    _logger.LogInformation("Waiting DoReceiveMessageOperation at: {time}", DateTimeOffset.Now);
                    Task.Delay(60000, stoppingToken).Wait();
                }

                _logger.LogInformation("************* Expire Time ************");
            }
            catch (AggregateException ae)
            {
                _logger.LogError("One or more exceptions occurred: ");
                foreach (var ex in ae.Flatten().InnerExceptions)
                    _logger.LogError("Exception {0}", ex.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(default, e, e.Message);
            }

            return Task.CompletedTask;
        }

        // event handler
        private void EventHandler_MessageReceived(object sender, string queueName)
        {
            _logger.LogInformation($"Process Completed! {queueName}");
            _logger.LogInformation($"Message {sender.ToString()}");

            if (QueueName.HISTORICAL_PRICE == queueName)
            {
                _historicalPriceService.SaveInDatabaseOperation(sender.ToString());
            }
            else if (QueueName.HISTORICAL_DIVIDENDS == queueName)
            {
                _historicalEarningService.SaveInDatabaseOperation(sender.ToString());
            }
            else
            {
                _logger.LogError($"Event Handler Message Received, queue: {queueName}, message {sender}");
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");
            
            if (messageBrokerService != null)
                messageBrokerService.Dispose();

            await base.StopAsync(stoppingToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation($"Worker disposed at: {DateTime.Now}");
            base.Dispose();
        }
    }

}
