using AutoMapper;
using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.Domain.Interfaces.Base;
using CtrlInvest.Infra.Repository;
using CtrlInvest.Services.Email;
using CtrlInvest.Services.Identity;
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
            services.AddScoped<ICustomEmailSender, CustomEmailSender>();
            services.AddScoped<IIdentityCustomService, IdentityCustomService>();
            services.AddScoped<IMapper, Mapper>();
            //services.AddScoped<IChildTreeAppService, ChildTreeAppService>();
            //services.AddScoped<IGrandChildTreeAppService, GrandChildTreeAppService>();
        }
    }
}
