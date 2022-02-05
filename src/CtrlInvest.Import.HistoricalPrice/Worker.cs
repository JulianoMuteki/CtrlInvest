using CtrlInvest.Import.HistoricalPrice.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace CtrlInvest.Import.HistoricalPrice
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ServiceConfigurations _serviceConfigurations;
        private IImportHistoricalPriceService _historicalPriceImportController;

        protected readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IImportHistoricalPriceService historicalPriceImportController)
        {
            //    _serviceProvider = serviceProvider;
            _historicalPriceImportController = historicalPriceImportController;
            _logger = logger;

            _serviceConfigurations = new ServiceConfigurations();
            new ConfigureFromConfigurationOptions<ServiceConfigurations>(
                configuration.GetSection("ServiceConfigurations"))
                    .Configure(_serviceConfigurations);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (string host in _serviceConfigurations.Hosts)
                {
                    try
                    {
                        using (Ping p = new Ping())
                        {
                            var resposta = p.Send(host);
                            // resultado.Status = resposta.Status.ToString();
                            _logger.LogInformation($"Checking host {host} - {0}", resposta.Status.ToString());
                        }

                        _historicalPriceImportController.DoImportOperation();                        
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                }

                _logger.LogInformation("************************************  Waiting time.. ************************************");
                 await Task.Delay(
                    _serviceConfigurations.Intervalo, stoppingToken);
            }
        }

    }
}
