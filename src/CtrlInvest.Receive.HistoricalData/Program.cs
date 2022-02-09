using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using CtrlInvest.Infra.Context;
using CtrlInvest.Domain.Interfaces.Base;
using CtrlInvest.Infra.Repository;
using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.Services;
using CtrlInvest.Receive.HistoricalData.Services;
using CtrlInvest.MessageBroker;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace CtrlInvest.Receive.HistoricalData
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            CreateDatabaseIfNotExist(host);

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
                    IConfiguration Configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables()
                            .AddCommandLine(args)
                            .Build();
                    services.AddDbContext<CtrlInvestContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Singleton);
                    RegisterServices(services);
                    services.AddHostedService<Worker>();
                });

        private static void RegisterServices(IServiceCollection services)
        {
            // Adding dependencies from another layers (isolated from Presentation)
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ITicketAppService, TicketAppService>();
            services.AddTransient<IHistoricalPriceService, HistoricalPriceService>();

            // Thread services
            services.AddScoped<IReceiveHistoryPriceService, ReceiveHistoryPriceService>();

            //Message Broker
            var configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();
            var rabbitConfig = configuration.GetSection("RabbitConfig");
            services.Configure<RabbitOptions>(rabbitConfig);

            services.AddScoped<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddScoped<IPooledObjectPolicy<IModel>, RabbitFactory>();
        }
    }
}
