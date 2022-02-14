using CtrlInvest.Import.Dividends.Services;
using CtrlInvest.MessageBroker;
using CtrlInvest.MessageBroker.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CtrlInvest.Import.Dividends
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;
        IList<Task> _works;
        public Worker(IServiceProvider services, ILogger<Worker> logger)
        {
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
            stoppingToken.ThrowIfCancellationRequested();

            _works.Add(StartProcessReceiveMessage(stoppingToken, "teste2"));
            _works.Add(StartProcessReceiveMessage(stoppingToken, QueueName.HISTORICAL_PRICE));

            try
            {
                Task.WaitAll(_works.ToArray());
            }
            catch (AggregateException ae)
            {
                _logger.LogError("One or more exceptions occurred: ");
                foreach (var ex in ae.Flatten().InnerExceptions)
                    _logger.LogError("Exception {0}", ex.Message);
            }
        }


        private async Task StartProcessReceiveMessage(CancellationToken stoppingToken, string queueName)
        {
            stoppingToken.ThrowIfCancellationRequested();

            using (var scope = _serviceProvider.CreateScope())
            {
                var messageBrokerService =
                    scope.ServiceProvider
                        .GetService<IMessageBrokerService>();

                // messageBrokerService.ProcessCompleted += EventHandler_MessageReceived;
                messageBrokerService.SetQueueChannel(queueName);
                await messageBrokerService.DoReceiveMessageOperation(stoppingToken);
               
            }
            await Task.CompletedTask;
        }

        // event handler
        private void EventHandler_MessageReceived(object sender, string queueName)
        {
            _logger.LogInformation($"Process Completed! ####### {queueName}");
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
