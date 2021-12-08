using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.Domain.Interfaces.Base;
using CtrlInvest.Infra.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace CtrlInvest.Services.Common
{
    public class BootStrapperModule
    {

        public static void RegisterServices(IServiceCollection services)
        {
            //helper service
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // services.AddScoped<NotificationContext, NotificationContext>();


            services.AddScoped<ITicketAppService, TicketAppService>();
            //services.AddScoped<IRegisterAppService, RegisterAppService>();
            //services.AddScoped<IParentTreeAppService, ParentTreeAppService>();
            //services.AddScoped<IChildTreeAppService, ChildTreeAppService>();
            //services.AddScoped<IGrandChildTreeAppService, GrandChildTreeAppService>();
        }
    }
}
