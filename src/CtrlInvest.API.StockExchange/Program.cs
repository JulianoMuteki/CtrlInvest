using CtrlInvest.CrossCutting.Ioc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
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
}
