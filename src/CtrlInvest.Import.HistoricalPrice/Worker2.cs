using CtrlInvest.Import.HistoricalPrice.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace CtrlInvest.Import.HistoricalPrice
{
    public class Worker2 : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ServiceConfigurations _serviceConfigurations;
        private IImportHistoricalPriceService _historicalPriceImportController;

        protected readonly IServiceProvider _serviceProvider;

        public Worker2(ILogger<Worker> logger, IConfiguration configuration, IImportHistoricalPriceService historicalPriceImportController)
        {
            //    _serviceProvider = serviceProvider;
            _historicalPriceImportController = historicalPriceImportController;
            _logger = logger;

            _serviceConfigurations = new ServiceConfigurations();
            new ConfigureFromConfigurationOptions<ServiceConfigurations>(
                configuration.GetSection("ServiceConfigurations"))
                    .Configure(_serviceConfigurations);
        }

        public override Task StartAsync(CancellationToken
              cancellationToken)
        {
            _logger.LogInformation
            ("Worker service has been started at: {0}", DateTime.Now);
            return base.StartAsync(cancellationToken);
        }
        protected override Task ExecuteAsync(CancellationToken
         stoppingToken)
        {
            _logger.LogInformation
            ("Worker service running at: {0}", DateTime.Now);
            return Task.CompletedTask;
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
