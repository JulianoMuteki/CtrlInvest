using CtrlInvest.Domain.Interfaces.Base;
using CtrlInvest.Infra.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace CtrlInvest.CrossCutting.Ioc
{
    public class InfraBootStrapperModule
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //helper service
            services.AddScoped<IUnitOfWork, UnitOfWork>();
           // services.AddScoped<NotificationContext, NotificationContext>();

        }
    }
}
