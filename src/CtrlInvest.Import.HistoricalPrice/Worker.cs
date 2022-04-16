using CtrlInvest.MessageBroker;
using CtrlInvest.MessageBroker.Common;
using CtrlInvest.MessageBroker.Helpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CtrlInvest.Import.HistoricalPrice
{
    public class Worker : BackgroundService
    {

        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMessageBrokerService _messageBrokerService;
        private readonly IImportHistoricalPriceService _importHistoricalPriceService;

        IList<Task> _works;

        public Worker(IServiceProvider services, ILogger<Worker> logger, IImportHistoricalPriceService historicalPriceImportController, IMessageBrokerService messageBrokerService)
        {
            _logger = logger;
            _importHistoricalPriceService = historicalPriceImportController;
            _importHistoricalPriceService.ThresholdReached += event_ImportDataFromServerCompleted;
            _messageBrokerService = messageBrokerService;
            _messageBrokerService.SetQueueChannel(QueueName.HISTORICAL_PRICE);

            _works = new List<Task>();
        }
        private void event_ImportDataFromServerCompleted(object sender, ImportDataFromServerEventArgs e)
        {
            _logger.LogInformation($"Event Import Data From Server Completed! {sender}");
            _works.Add(SendToMessageBroker(e.HistoricalDataMessage, e.Ticket.Ticker, e.Ticket.Id));
        }

        Task SendToMessageBroker(string historicalPriceList, string ticketCode, Guid ticketID)
        {
            var task = Task.Run(() =>
            {
                var messages = MessageBrokerParse.ConvertStringToList(historicalPriceList, ticketCode, ticketID);
                _logger.LogInformation($"Sending Messages to broker, total messages: {messages.Count}");

                foreach (var item in messages)
                {
                    _logger.LogInformation($"Sending: {item.ToJson()}");
                    _messageBrokerService.DoSendMessageOperation(item.ToJson());
                }
            }).ContinueWith(t =>
            {
                _logger.LogError("Error in messageBrokerService.DoSendMessageOperation");
                _logger.LogError("{0}: {1}",
                                    t.Exception.InnerException.GetType().Name,
                                    t.Exception.InnerException.Message);
            }, TaskContinuationOptions.OnlyOnFaulted);

            return task;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation
            ("Worker service has been started at: {0}", DateTime.Now);
            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running.");

            try
            {
                stoppingToken.ThrowIfCancellationRequested();

                DateTime dateTime = DateTime.Now.AddHours(-3);

                while (!stoppingToken.IsCancellationRequested)
                {
                    if (DateTime.Now > dateTime.AddMinutes(5))
                    {
                        if (_works.Count > 0)
                        {
                            try
                            {
                                await Task.WhenAll(_works);
                            }
                            catch { }
                            _works.Clear();
                        }

                        await StartProcessDownloadHistoricalPrices(stoppingToken);
                        dateTime = DateTime.Now;
                    }
                    _logger.LogInformation("Receiving running at: {time}", DateTimeOffset.Now);
                    //await Task.Delay(864 * 100000, stoppingToken);
                    await Task.Delay(60000, stoppingToken);
                }
            }
            catch (AggregateException ae)
            {
                _logger.LogError("One or more exceptions occurred: ");
                foreach (var ex in ae.Flatten().InnerExceptions)
                    _logger.LogError("Exception {0}", ex.Message);
            }
            catch (OperationCanceledException e)
            {
                _logger.LogError($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(e)} thrown with message: {e.Message}");
            }

            await Task.CompletedTask;
        }

        private async Task StartProcessDownloadHistoricalPrices(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            _importHistoricalPriceService.DoImportOperation();

            await Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation
            ("Worker service has been stopped at: {0}", DateTime.Now);
            return base.StopAsync(cancellationToken);
        }
        public override void Dispose()
        {
            _logger.LogInformation
            ("Worker service has been disposed at: {0}", DateTime.Now);
            base.Dispose();
        }
    }
}
