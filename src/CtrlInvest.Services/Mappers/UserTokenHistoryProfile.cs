using AutoMapper;
using CtrlInvest.Domain.Entities.Aggregates;
using CtrlInvest.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlInvest.Services.Mappers
{
    internal class UserTokenHistoryProfile : Profile
    {
        public UserTokenHistoryProfile()
        {
            CreateMap<UserTokenHistoryViewModel, UserTokenHistory>()
                 .ForMember(
                    dest => dest.JwtId,
                    opt => opt.MapFrom(src => $"{src.JwtId}")
                )
                .ForMember(
                    dest => dest.UserId,
                    opt => opt.MapFrom(src => $"{src.UserId}")
                )
                .ForMember(
                    dest => dest.Token,
                    opt => opt.MapFrom(src => $"{src.Token}")
                );

            CreateMap<UserTokenHistory, UserTokenHistoryViewModel>()
                     .ForMember(
                        dest => dest.JwtId,
                        opt => opt.MapFrom(src => $"{src.JwtId}")
                    )
                    .ForMember(
                        dest => dest.UserId,
                        opt => opt.MapFrom(src => $"{src.UserId}")
                    )
                    .ForMember(
                        dest => dest.Token,
                        opt => opt.MapFrom(src => $"{src.Token}")
                    );
        }
    }
}