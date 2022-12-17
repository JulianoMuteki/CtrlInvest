using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;

namespace CtrlInvest.CrossCutting.Ioc
{
    public static class SerilogConfiguration
    {
		public static void ConfigureLogging(IConfigurationRoot configuration)
		{
			var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			Log.Logger = new LoggerConfiguration()
						.Enrich.FromLogContext()
						.WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
						.Enrich.WithProperty("Environment", environment)
						.WriteTo.Console()
						.CreateLogger();
		}

		private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
		{
			return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
			{
				AutoRegisterTemplate = true,
				AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
				IndexFormat = $"ctrlinvest.api.stockexchange-{environment.ToLower()}-{DateTime.Now:yyyy.MM}"
			};
		}
	}
}
