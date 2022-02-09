using CtrlInvest.Import.Dividends.Services;
using CtrlInvest.MessageBroker;
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

            _works.Add(StartProcessReceiveMessage(stoppingToken));
            // works.Add(DoWork2(stoppingToken));

            try
            {
                await Task.WhenAll(_works);
            }
            catch (AggregateException ae)
            {
                throw ae.Flatten();
            }

        }

        private async Task StartProcessReceiveMessage(CancellationToken stoppingToken)
        {
            await Task.Factory.StartNew(() =>
            {
                _logger.LogInformation(
                "Consume Scoped Service Hosted Service is working.");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var messageBrokerService =
                        scope.ServiceProvider
                            .GetRequiredService<IMessageBrokerService>();

                    messageBrokerService.ProcessCompleted += EventHandler_MessageReceived;
                    messageBrokerService.DoReceiveOperation();
                }
            });          
        }

        // event handler
        private void EventHandler_MessageReceived(object sender, EventArgs e)
        {
            _logger.LogInformation("Process Completed!");
        }

        //private async Task DoWork2(CancellationToken stoppingToken)
        //{
        //    _logger.LogInformation(
        //        "Consume Scoped Service Hosted Service is working.");

        //    using (var scope = Services.CreateScope())
        //    {
        //        var scopedProcessingService =
        //            scope.ServiceProvider
        //                .GetRequiredService<IScopedProcessingService>();

        //        await scopedProcessingService.DoService(stoppingToken, "worker 2");
        //    }
        //}

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
