using AutoMapper;
using CtrlInvest.Domain.Entities;
using CtrlInvest.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Services.Mappers
{
    internal class HistoricalPriceProfile : Profile
    {
        public HistoricalPriceProfile()
        {
            CreateMap<HistoricalPriceDto, HistoricalPrice>()
                     .ForMember(
                        dest => dest.Date,
                        opt => opt.MapFrom(src => src.Date)
                    )
                    .ForMember(
                        dest => dest.Open,
                        opt => opt.MapFrom(src => src.Open)
                    )
                    .ForMember(
                        dest => dest.High,
                        opt => opt.MapFrom(src => src.High)
                    )
                    .ForMember(
                        dest => dest.Low,
                        opt => opt.MapFrom(src => src.Low)
                    )
                    .ForMember(
                        dest => dest.AdjClose,
                        opt => opt.MapFrom(src => src.AdjClose)
                    )
                    .ForMember(
                        dest => dest.Volume,
                        opt => opt.MapFrom(src => src.Volume)
                    )
                   .ForMember(
                        dest => dest.TickerCode,
                        opt => opt.MapFrom(src => src.TickerCode));

            CreateMap<HistoricalPrice, HistoricalPriceDto>()
                     .ForMember(
                        dest => dest.Date,
                        opt => opt.MapFrom(src => src.Date)
                    )
                    .ForMember(
                        dest => dest.Open,
                        opt => opt.MapFrom(src => src.Open)
                    )
                    .ForMember(
                        dest => dest.High,
                        opt => opt.MapFrom(src => src.High)
                    )
                    .ForMember(
                        dest => dest.Low,
                        opt => opt.MapFrom(src => src.Low)
                    )
                    .ForMember(
                        dest => dest.AdjClose,
                        opt => opt.MapFrom(src => src.AdjClose)
                    )
                    .ForMember(
                        dest => dest.Volume,
                        opt => opt.MapFrom(src => src.Volume)
                    )
                   .ForMember(
                        dest => dest.TickerCode,
                        opt => opt.MapFrom(src => src.TickerCode));
        }
    }
}
