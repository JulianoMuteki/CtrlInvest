using CtrlInvest.Infra.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace CtrlInvest.API.StockExchange
{
    public class MigrationStartupFilter<TContext> : IStartupFilter where TContext : CtrlInvestContext
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                using (var scope = app.ApplicationServices.CreateScope())
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
                        else if (dbContext.Database.GetService<Microsoft.EntityFrameworkCore.Storage.IRelationalDatabaseCreator>().CanConnect())
                        {
                            dbContext.GetInfrastructure().GetService<IMigrator>().Migrate();
                        }
                    }
                    catch(Npgsql.NpgsqlException ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred connect to postgres database.");
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while seeding the database.");
                    }
                }
                next(app);
            };
        }
    }
}
