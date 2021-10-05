using EagleRockHub.Entities;
using EagleRockHub.Interfaces;
using EagleRockHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EagleRockHub.Repositories
{
    public class EagleHubRepository : IEagleHubRepository
    {
        private readonly ICacheProviderService _cacheProviderService;

        public EagleHubRepository(ICacheProviderService cacheProviderService)
        {
            _cacheProviderService = cacheProviderService;
        }

        public async Task AddTrafficStatistics(TrafficStatistics _trafficStatics)
        {
            await _cacheProviderService.AddItem(CacheKeys.TrafficStatisticsKey, _trafficStatics);
        }

        public async Task<List<TrafficStatistics>> GetTrafficStatistics()
        {
            var trafficStatistics = await _cacheProviderService.GetFromCache<List<TrafficStatistics>>(CacheKeys.TrafficStatisticsKey);
            return trafficStatistics;
        }
    }
}
