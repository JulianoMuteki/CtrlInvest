
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CtrlInvest.Infra.Context;
using System;
using Microsoft.EntityFrameworkCore;
using CtrlInvest.CrossCutting.Ioc;
using CtrlInvest.Domain.Interfaces.Application;
using System.IO;

namespace CtrlInvest.ImportHistorical
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, args);
            RegisterServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            Context context;
            // Three contexts following different strategies
            context = new Context(new ConcreteStrategyA(), serviceProvider);
            context.ImportHistoricalByDates();
            // Wait for user
            Console.ReadKey();
        }

        public static void ConfigureServices(IServiceCollection services, string[] args)
        {
            IConfiguration Configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables()
              .AddCommandLine(args)
            .Build();
            //   IConfiguration Configuration = new ConfigurationBuilder().
            //SetBasePath(Directory.GetCurrentDirectory()).
            //AddJsonFile("appsettings.json").
            //Build();

            services.AddDbContext<CtrlInvestContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

        }

        private static void RegisterServices(IServiceCollection services)
        {
            // Adding dependencies from another layers (isolated from Presentation)
            InfraBootStrapperModule.RegisterServices(services);
            ApplicationBootStrapperModule.RegisterServices(services);
        }
    }
}
