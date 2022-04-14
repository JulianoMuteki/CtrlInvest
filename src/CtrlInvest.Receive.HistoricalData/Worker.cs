using CtrlInvest.MessageBroker;
using CtrlInvest.MessageBroker.Helpers;
using CtrlInvest.Services.StocksExchanges;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
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
        private readonly ConcurrentQueue<string> _queue;
        private readonly RequestQueue _requestQueue;

        IMessageBrokerService messageBrokerService = null;
        private CancellationTokenSource cancelTokenSourceWorker = new CancellationTokenSource();
        private CancellationToken ctOperation;
        public Worker(IServiceProvider services, ILogger<Worker> logger, IHistoricalPriceService historicalPriceService, IHistoricalEarningService historicalEarningService)
        {
            _queue = new ConcurrentQueue<string>();
            _historicalPriceService = historicalPriceService;
            _historicalEarningService = historicalEarningService;
            _serviceProvider = services;
            _logger = logger;
            _requestQueue = RequestQueue.Instance;
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
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service running.");
            var tasks = new List<Task>();

            while (!stoppingToken.IsCancellationRequested)
            {
                stoppingToken.ThrowIfCancellationRequested();

                tasks.Add(StartProcessImportHistoricalPrice(ctOperation));
              //  tasks.Add(StartProcessImportDividend(ctOperation));
                Task.WaitAll(tasks.ToArray(), stoppingToken);
                //Tempo para rodar Broker novamente
                //5 horas
                await Task.Delay(60000 * 5, stoppingToken);
            }

            await Task.CompletedTask;
        }

        private async Task StartProcessImportHistoricalPrice(CancellationToken ctOperation)
        {
            await Task.Run(async () =>
            {
                await StartProcessReceiveMessage(ctOperation, QueueName.HISTORICAL_PRICE);
            }, ctOperation);
        }

        private async Task StartProcessImportDividend(CancellationToken ctOperation)
        {
            await Task.Run(async () =>
            {
                await StartProcessReceiveMessage(ctOperation, QueueName.HISTORICAL_DIVIDENDS);
            }, ctOperation);
        }

        private Task StartProcessReceiveMessage(CancellationToken ctOperation, string queueName)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();

                messageBrokerService = scope.ServiceProvider
                                                .GetService<IMessageBrokerService>();

                messageBrokerService.ProcessCompleted += EventHandler_MessageReceived;
                messageBrokerService.SetQueueChannel(queueName);
                messageBrokerService.DoReceiveMessageOperation();

                _requestQueue.Launch(_historicalPriceService, _historicalEarningService);

                while (!messageBrokerService.isExpireTimeToReceiveMessage())
                {
                    ctOperation.ThrowIfCancellationRequested();

                    _logger.LogInformation("Waiting DoReceiveMessageOperation at: {time}", DateTimeOffset.Now);
                    Task.Delay(6000, ctOperation).Wait();
                }

                _logger.LogInformation("************* Expire Time ************");
            }
            catch (AggregateException ae)
            {
                _logger.LogError("One or more exceptions occurred: ");
                foreach (var ex in ae.Flatten().InnerExceptions)
                    _logger.LogError("Exception {0}", ex.Message);
            }
            catch (OperationCanceledException opX)
            {
                _logger.LogError(default, opX, opX.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(default, e, e.Message);
            }
            finally
            {
                messageBrokerService.Dispose();
                _requestQueue.StopLaunch();
            }

            return Task.CompletedTask;
        }

        // event handler
        private void EventHandler_MessageReceived(object sender, string queueName)
        {            
            _requestQueue.Add(sender.ToString());
            _logger.LogInformation($"QueueName {queueName}, Message: {sender.ToString()}");

            //if (QueueName.HISTORICAL_PRICE == queueName)
            //{
            //    _historicalPriceService.SaveInDatabaseOperation(sender.ToString());
            //}
            //else if (QueueName.HISTORICAL_DIVIDENDS == queueName)
            //{
            //    _historicalEarningService.SaveInDatabaseOperation(sender.ToString());
            //}
            //else
            //{
            //    _logger.LogError($"Event Handler Message Received, queue: {queueName}, message {sender}");
            //}
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            if (messageBrokerService != null)
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
    }

}
