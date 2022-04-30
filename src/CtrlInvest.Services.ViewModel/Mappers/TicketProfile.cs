using AutoMapper;
using CtrlInvest.Domain.Entities;

namespace CtrlInvest.Services.ViewModel
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<TicketDto, Ticket>()
                 .ForMember(
                    dest => dest.Ticker,
                    opt => opt.MapFrom(src => $"{src.Ticker}")
                )
                .ForMember(
                    dest => dest.Exchange,
                    opt => opt.MapFrom(src => $"{src.Exchange}")
                )
                .ForMember(
                    dest => dest.Country,
                    opt => opt.MapFrom(src => $"{src.Country}")
                )
                .ForMember(
                    dest => dest.Currency,
                    opt => opt.MapFrom(src => $"{src.Currency}")
                );

            CreateMap<Ticket, TicketDto>()
                 .ForMember(
                    dest => dest.Ticker,
                    opt => opt.MapFrom(src => $"{src.Ticker}")
                )
                .ForMember(
                    dest => dest.Exchange,
                    opt => opt.MapFrom(src => $"{src.Exchange}")
                )
                .ForMember(
                    dest => dest.Country,
                    opt => opt.MapFrom(src => $"{src.Country}")
                )
                .ForMember(
                    dest => dest.Currency,
                    opt => opt.MapFrom(src => $"{src.Currency}")
                );
        }
    }
}