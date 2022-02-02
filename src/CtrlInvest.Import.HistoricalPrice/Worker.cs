using CtrlInvest.Import.HistoricalPrice.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Worker(ILogger<Worker> logger,
            IConfiguration configuration)
        {
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
                _logger.LogInformation("Worker executando em: {time}",
                    DateTimeOffset.Now);

                foreach (string host in _serviceConfigurations.Hosts)
                {
                    _logger.LogInformation(
                        $"Verificando a disponibilidade do host {host}");

                    try
                    {
                        using (Ping p = new Ping())
                        {
                            var resposta = p.Send(host);
                            // resultado.Status = resposta.Status.ToString();
                            _logger.LogInformation(resposta.Status.ToString());
                        }

                        //   bool result = await PingWithHttpClient(host);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }

                }

                await Task.Delay(
                    _serviceConfigurations.Intervalo, stoppingToken);
            }
        }

        private async Task<bool> PingWithHttpClient(string hostUrl)
        {
            var httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(hostUrl),
                Method = HttpMethod.Head
            };
            var result = await httpClient.SendAsync(request);          
            return result.IsSuccessStatusCode;
        }
    }
}
