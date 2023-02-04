using AutoMapper;
using CtrlInvest.Domain.Entities;

namespace CtrlInvest.Services.ViewModel
{
    public class TicketSyncProfile : Profile
    {
        public TicketSyncProfile()
        {
            CreateMap<TicketSyncDto, TicketSync>()
                .ForMember(
                    dest => dest.DateStart,
                    opt => opt.MapFrom(src => $"{src.DateStart}")
                );

            CreateMap<TicketSync, TicketSyncDto>()
                .ForMember(
                    dest => dest.DateStart,
                    opt => opt.MapFrom(src => $"{src.DateStart}")
                )
                .ForMember(
                    dest => dest.Ticker,
                    opt => opt.MapFrom(src => $"{src.Ticket.Ticker}")
                );
        }
    }
}