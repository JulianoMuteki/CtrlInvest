using CtrlInvest.MessageBroker;
using CtrlInvest.MessageBroker.Helpers;
using CtrlInvest.Services.StocksExchanges;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private IList<Task> _works;
        public Worker(IServiceProvider services, ILogger<Worker> logger, IHistoricalPriceService historicalPriceService, IHistoricalEarningService historicalEarningService)
        {
            _historicalPriceService = historicalPriceService;
            _historicalEarningService = historicalEarningService;
            _serviceProvider = services;
            _logger = logger;
            _works = new List<Task>();
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

            try
            {
                _works.Add(StartProcessReceiveMessage(stoppingToken, QueueName.HISTORICAL_DIVIDENDS));
                _works.Add(StartProcessReceiveMessage(stoppingToken, QueueName.HISTORICAL_PRICE));

                await Task.WhenAll(_works);
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


            await Task.CompletedTask;
        }

        private async Task StartProcessReceiveMessage(CancellationToken stoppingToken, string queueName)
        {
            try
            {
                stoppingToken.ThrowIfCancellationRequested();

                using var scope = _serviceProvider.CreateScope();

                var messageBrokerService = scope.ServiceProvider
                                                .GetService<IMessageBrokerService>();

                messageBrokerService.ProcessCompleted += EventHandler_MessageReceived;
                messageBrokerService.SetQueueChannel(queueName);
                messageBrokerService.DoReceiveMessageOperation();

                while (!stoppingToken.IsCancellationRequested)
                {
                    stoppingToken.ThrowIfCancellationRequested();

                    _logger.LogInformation("while Waiting receive running at: {time}", DateTimeOffset.Now);
                    await Task.Delay(6000, stoppingToken);
                }
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

            await Task.CompletedTask;
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

            await base.StopAsync(stoppingToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation($"Worker disposed at: {DateTime.Now}");
            base.Dispose();
        }
    }

}
