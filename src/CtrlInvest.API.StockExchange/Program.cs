using CtrlInvest.CrossCutting.Ioc;
using CtrlInvest.Infra.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace CtrlInvest.API.StockExchange
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                IHost host = CreateHostBuilder(args).Build();
                // CreateDbIfNotExist(host);         
               
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                host.Run();
                
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host encerrado inesperadamente");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
            return 0;
        }

        private static void CreateDbIfNotExist(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var dbContext = scope.ServiceProvider.GetService<CtrlInvestContext>();

                    if (!dbContext.Database.GetService<Microsoft.EntityFrameworkCore.Storage.IRelationalDatabaseCreator>().Exists())
                    {
                        dbContext.GetInfrastructure().GetService<IMigrator>().Migrate();
                        new DbInitializer().Initialize(services);
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var settings = config.Build();
                    SerilogConfiguration.ConfigureLogging(settings);
                })
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public static class SentryHostBuilderExtensions
    {

        public static IHost MigrateDatabase(this IHost host)
        {
            IServiceProvider services = host.Services;

            using (var serviceScope = services.CreateScope())
            {
                try
                {
                    var context = serviceScope.ServiceProvider.GetService<CtrlInvestContext>();
                    // context.GetInfrastructure().GetService<IMigrator>().Migrate();
                    if (context.Database.EnsureCreated())
                        new DbInitializer().Initialize(serviceScope.ServiceProvider);
                }

                catch (Exception ex)
                {
                    var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                    throw;
                }
            }
            return host;
        }
    }
}
