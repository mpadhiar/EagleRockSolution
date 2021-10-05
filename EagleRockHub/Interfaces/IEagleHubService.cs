using EagleRockHub.Dtos;
using EagleRockHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EagleRockHub.Interfaces
{
    public interface IEagleHubService
    {
        Task<ApiResponse> AddTrafficStatistics(TrafficStatisticsDto trafficStatisticDto);
    }
}
