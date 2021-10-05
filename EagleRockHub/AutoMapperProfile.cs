using AutoMapper;
using EagleRockHub.Dtos;
using EagleRockHub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EagleRockHub
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TrafficStatisticsDto, TrafficStatistics>();

            CreateMap<TrafficStatistics, TrafficStatisticsDto>();
        }
    }
}
