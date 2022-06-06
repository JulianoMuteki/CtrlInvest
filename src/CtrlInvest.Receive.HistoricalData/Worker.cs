using CtrlInvest.MessageBroker;
using CtrlInvest.MessageBroker.Helpers;
using CtrlInvest.Services.StocksExchanges;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        IMessageBrokerService _messageBrokerService = null;
        private CancellationTokenSource cancelTokenSourceWorker = new CancellationTokenSource();
        private CancellationToken ctOperation;
        public Worker(IServiceProvider services, ILogger<Worker> logger, IHistoricalPriceService historicalPriceService, IHistoricalEarningService historicalEarningService)
        {
            _historicalPriceService = historicalPriceService;
            _historicalEarningService = historicalEarningService;
            _serviceProvider = services;
            _logger = logger;
            ctOperation = cancelTokenSourceWorker.Token;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Worker started at: {DateTime.Now}");
            _logger.LogInformation("Database configured");
            await Task.Delay(5000, cancellationToken);
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ExecuteAsync");
            var tasks = new List<Task>();            

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Running ExecuteAsync");

                    tasks.Add(StartProcessImportHistoricalPrice(ctOperation));
                    tasks.Add(StartProcessImportDividend(ctOperation));
                    Task.WaitAll(tasks.ToArray(), stoppingToken);

                    stoppingToken.ThrowIfCancellationRequested();
                    //Tempo para rodar Broker novamente
                    //5 horas
                    await Task.Delay(60000 * 5, stoppingToken);
                }
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogCritical(default, ex, ex.Message);
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogCritical(default, ex, ex.Message);
                throw;
            }

            await Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            if (_messageBrokerService != null)
            {
                cancelTokenSourceWorker.Cancel();
            }

            await base.StopAsync(stoppingToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation($"Worker disposed at: {DateTime.Now}");
            base.Dispose();
        }

        private async Task StartProcessImportHistoricalPrice(CancellationToken ctOperation)
        {
            await Task.Run(() =>
            {
                ExecuteProcessReceiveMessage(ctOperation, QueueName.HISTORICAL_PRICE);
            }, ctOperation);
        }

        private async Task StartProcessImportDividend(CancellationToken ctOperation)
        {
            await Task.Run(() =>
            {
                ExecuteProcessReceiveMessage(ctOperation, QueueName.HISTORICAL_DIVIDENDS);
            }, ctOperation);
        }

        private void ExecuteProcessReceiveMessage(CancellationToken ctOperation, string queueName)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                _messageBrokerService = scope.ServiceProvider
                                                .GetService<IMessageBrokerService>();

                if (QueueName.HISTORICAL_PRICE == queueName)
                    _messageBrokerService.ProcessCompleted += EventHandler_HistoricalPrice_MessageReceived;
                else if (QueueName.HISTORICAL_DIVIDENDS == queueName)
                    _messageBrokerService.ProcessCompleted += EventHandler_Earning_MessageReceived;

                _messageBrokerService.SetQueueChannel(queueName);
                _messageBrokerService.DoReceiveMessageOperation();

                while (!_messageBrokerService.isExpireTimeToReceiveMessage())
                {
                    if (ctOperation.IsCancellationRequested)
                        ctOperation.ThrowIfCancellationRequested();

                    _logger.LogInformation("{queueName}  - Running ProcessReceiveMessage in MessageBrokerService.DoReceiveMessageOperation at: {time} ", queueName, DateTimeOffset.Now);
                    // Minutes
                    Thread.Sleep(60000 * 1);
                }

                _logger.LogInformation("************* MessageBrokerService ExpireTimeToReceiveMessage ************");
            }
            catch (AggregateException ae)
            {
                _logger.LogError("One or more exceptions occurred: ");
                foreach (var ex in ae.Flatten().InnerExceptions)
                    _logger.LogError("Exception {0}", ex.Message);
                throw;
            }
            catch (OperationCanceledException opX)
            {
                _logger.LogError(default, opX, opX.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogCritical(default, e, e.Message);
                throw;
            }
            finally
            {
                _messageBrokerService.Dispose();
            }
        }

        // event handler
        private void EventHandler_HistoricalPrice_MessageReceived(object sender, string queueName)
        {
            _historicalPriceService.SaveInDatabaseOperation(sender.ToString());
        }

        private void EventHandler_Earning_MessageReceived(object sender, string queueName)
        {
            _historicalEarningService.SaveInDatabaseOperation(sender.ToString());
        }

    }

}
