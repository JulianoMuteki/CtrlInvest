
using CtrlInvest.Import.Dividends.Services;
using CtrlInvest.MessageBroker;
using CtrlInvest.MessageBroker.Common;
using CtrlInvest.MessageBroker.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CtrlInvest.Import.Dividends
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMessageBrokerService _messageBrokerService;
        private readonly IEarningService _importHistoricalEarningService;

        IList<Task> _works;
        public Worker(IServiceProvider services, ILogger<Worker> logger, IEarningService importHistoricalEarningService, IMessageBrokerService messageBrokerService)
        {
            _serviceProvider = services;
            _importHistoricalEarningService = importHistoricalEarningService;
            _importHistoricalEarningService.ThresholdReached += event_ImportDataFromServerCompleted;
            _messageBrokerService = messageBrokerService;
            _messageBrokerService.SetQueueChannel(QueueName.HISTORICAL_DIVIDENDS);

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
            _logger.LogInformation("Worker running.");

            try
            {
                stoppingToken.ThrowIfCancellationRequested();

                DateTime dateTime = DateTime.Now.AddHours(-3);

                while (!stoppingToken.IsCancellationRequested)
                {
                    if (DateTime.Now > dateTime.AddHours(1))
                    {
                        if (_works.Count > 0)
                        {
                            await Task.WhenAll(_works);
                            _works.Clear();
                        }

                        await StartProcessDownloadEarnings(stoppingToken);
                        dateTime = DateTime.Now;
                    }
                    _logger.LogInformation("Receiving running at: {time}", DateTimeOffset.Now);
                    //await Task.Delay(864 * 100000, stoppingToken);
                    await Task.Delay(300000, stoppingToken);
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


        private async Task StartProcessDownloadEarnings(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            _importHistoricalEarningService.DoImportOperation();

            await Task.CompletedTask;
        }

        // event handler
        private void event_ImportDataFromServerCompleted(object sender, ImportDataFromServerEventArgs e)
        {
            _logger.LogInformation($"####### Event Import Data From Server Completed! {sender}");

            _works.Add(SendToMessageBroker(e.HistoricalDataList, e.Ticket.Ticker, e.Ticket.Id));
        }

        Task SendToMessageBroker(string historicalEarningList, string ticketCode, Guid ticketID)
        {
            var task = Task.Run(() =>
            {
                var messages = MessageBrokerParse.ConvertStringToList(historicalEarningList, ticketCode, ticketID);
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

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            //TODO: add timeout
            await Task.WhenAll(_works);

            await base.StopAsync(stoppingToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation($"Worker disposed at: {DateTime.Now}");
            _messageBrokerService.Dispose();

            base.Dispose();
        }
    }
}
