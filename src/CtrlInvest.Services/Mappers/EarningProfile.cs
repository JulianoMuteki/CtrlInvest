using AutoMapper;
using CtrlInvest.Domain.Entities.StocksExchanges;
using CtrlInvest.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Services.Mappers
{
    internal class EarningProfile : Profile
    {
        public EarningProfile()
        {
            CreateMap<EarningDto, Earning>()
                         .ForMember(
                            dest => dest.DateWith,
                            opt => opt.MapFrom(src => src.DateWith)
                        )
                        .ForMember(
                            dest => dest.ValueIncome,
                            opt => opt.MapFrom(src => src.ValueIncome)
                        )
                        .ForMember(
                            dest => dest.Type,
                            opt => opt.MapFrom(src => src.Type)
                        )
                        .ForMember(
                            dest => dest.PaymentDate,
                            opt => opt.MapFrom(src => src.PaymentDate)
                        )
                        .ForMember(
                            dest => dest.Quantity,
                            opt => opt.MapFrom(src => src.Quantity)
                        );

            CreateMap<Earning, EarningDto>()
                         .ForMember(
                            dest => dest.DateWith,
                            opt => opt.MapFrom(src => src.DateWith)
                        )
                        .ForMember(
                            dest => dest.ValueIncome,
                            opt => opt.MapFrom(src => src.ValueIncome)
                        )
                        .ForMember(
                            dest => dest.Type,
                            opt => opt.MapFrom(src => src.Type)
                        )
                        .ForMember(
                            dest => dest.PaymentDate,
                            opt => opt.MapFrom(src => src.PaymentDate)
                        )
                        .ForMember(
                            dest => dest.Quantity,
                            opt => opt.MapFrom(src => src.Quantity)
                        );
        }
    }
}
