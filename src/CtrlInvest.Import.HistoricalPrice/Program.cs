using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using CtrlInvest.Infra.Context;
using CtrlInvest.Domain.Interfaces.Base;
using CtrlInvest.Infra.Repository;
using CtrlInvest.MessageBroker;
using CtrlInvest.Services.StocksExchanges;
using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.Services;

namespace CtrlInvest.Import.HistoricalPrice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            CreateDatabaseIfNotExist(host);
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            host.Run();
        }
        private static void CreateDatabaseIfNotExist(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<CtrlInvestContext>();
                    context.Database.EnsureCreated();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = new ConfigurationBuilder()
                                     .SetBasePath(hostContext.HostingEnvironment.ContentRootPath)
                                     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                     .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true)
                                     .AddEnvironmentVariables()
                                     .AddCommandLine(args)
                                     .Build();

                    services.AddDbContext<CtrlInvestContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Singleton);
                    RegisterServices(services, configuration);
                    services.AddHostedService<Worker>();                    
                });

        private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ITicketAppService, TicketAppService>();

            services.Configure<HostOptions>(configuration.GetSection("HostOptions"));
            var rabbitConfig = configuration.GetSection("RabbitConfig");
            services.Configure<RabbitOptions>(rabbitConfig);           

            services.AddSingleton<IRabbitFactoryConnection, RabbitFactory>();

            services.AddSingleton<IMessageBrokerService, MessageBrokerService>();
            services.AddTransient<IHistoricalPriceService, HistoricalPriceService>();
            services.AddTransient<IImportHistoricalPriceService, ImportHistoricalPriceService>();
        }
    }
}
