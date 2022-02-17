
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
            _importHistoricalEarningService.ThresholdReached += c_ThresholdReached;
            _messageBrokerService = messageBrokerService;
            // messageBrokerService.ProcessCompleted += EventHandler_MessageReceived;
            _messageBrokerService.SetQueueChannel(QueueName.HISTORICAL_DIVIDENDS);
            //await messageBrokerService.DoReceiveMessageOperation(stoppingToken);



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
                stoppingToken.ThrowIfCancellationRequested();

                while (!stoppingToken.IsCancellationRequested)
                {
                    if (_works.Count > 0)
                    {
                        await Task.WhenAll(_works);
                        _works.Clear();
                    }

                    await StartProcessDownloadEarnings(stoppingToken);

                    _logger.LogInformation("Receiving running at: {time}", DateTimeOffset.Now);
                    await Task.Delay(864 * 100000, stoppingToken);
                }
            }
            catch (AggregateException ae)
            {
                _logger.LogError("One or more exceptions occurred: ");
                foreach (var ex in ae.Flatten().InnerExceptions)
                    _logger.LogError("Exception {0}", ex.Message);
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
        private void c_ThresholdReached(object sender, ThresholdReachedEventArgs e)
        {
            _logger.LogInformation($"Process Completed! ####### {sender}");

            _works.Add(Task.Run(() => SendoToMessageBroker(e.HistoricalEarningList, e.Ticket.Ticker, e.Ticket.Id)));
        }

        void SendoToMessageBroker(string historicalEarningList, string ticketCode, Guid ticketID)
        {
            var messages = MessageBrokerParse.ConvertStringToList(historicalEarningList, ticketCode, ticketID);

            foreach (var item in messages)
            {
                _messageBrokerService.DoSendMessageOperation(item.ToJson());
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
