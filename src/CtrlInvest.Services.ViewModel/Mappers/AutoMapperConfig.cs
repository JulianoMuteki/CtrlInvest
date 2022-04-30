using AutoMapper;

namespace CtrlInvest.Services.ViewModel.Mappers
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TicketProfile());
            });
        }
    }
}
