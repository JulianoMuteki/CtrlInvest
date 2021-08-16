using CtrlInvest.Domain.Interfaces.Application;
using Microsoft.Extensions.DependencyInjection;

namespace CtrlInvest.CrossCutting.Ioc
{
    public class ApplicationBootStrapperModule
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //services.AddScoped<IBankAppService, BankAppService>();
            //services.AddScoped<IRegisterAppService, RegisterAppService>();
            //services.AddScoped<IParentTreeAppService, ParentTreeAppService>();
            //services.AddScoped<IChildTreeAppService, ChildTreeAppService>();
            //services.AddScoped<IGrandChildTreeAppService, GrandChildTreeAppService>();
        }
    }
}
