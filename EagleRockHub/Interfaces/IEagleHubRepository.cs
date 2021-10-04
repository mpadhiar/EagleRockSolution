using EagleRockHub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EagleRockHub.Interfaces
{
    public interface IEagleHubRepository
    {
        Task AddTrafficStatistics(TrafficStatistics _trafficStatics);
        Task<List<TrafficStatistics>> GetTrafficStatistics();
    }
}
