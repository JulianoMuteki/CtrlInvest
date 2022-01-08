using CtrlInvest.Domain.Interfaces.Application;
using CtrlInvest.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CtrlInvest.CrossCutting.Ioc
{
    public class ApplicationBootStrapperModule
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ITicketAppService, TicketAppService>();
            services.AddScoped<IInvestPortfolioService, InvestPortfolioService>();
            //services.AddScoped<IParentTreeAppService, ParentTreeAppService>();
            //services.AddScoped<IChildTreeAppService, ChildTreeAppService>();
            //services.AddScoped<IGrandChildTreeAppService, GrandChildTreeAppService>();
        }
    }
}
