using EagleRockHub.Entities;
using EagleRockHub.Services;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EagleRockHub.UniTests
{
    public class CacheProviderServiceTest
    {
        private readonly Mock<IDistributedCache> _distributeCache;

        public CacheProviderServiceTest()
        {
            _distributeCache = new Mock<IDistributedCache>();
        }

        [Fact]
        public async Task GetFromCache_ShouldReturnNull_IfKeyDoesntExistInCache()
        {
            string testKey = "_InvalidKey";

            _distributeCache.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                        .Returns(Task.FromResult<byte[]>(null));

            //Act
            var cacheService = new CacheProviderService(_distributeCache.Object);
            var cachedResource = await cacheService.GetFromCache<List<TrafficStatistics>>(testKey);

            //Assert
            Assert.Null(cachedResource);
        }

        [Fact]
        public async Task GetFromCache_ShouldReturnObject_IfKeyExistsInCache()
        {
            string testKey = "_Validkey";
            var testKeyValue = GetFakeTrafficStats();

            _distributeCache.Setup(x => x.GetAsync(testKey, It.IsAny<CancellationToken>()))
                                        .Returns(Task.FromResult<byte[]>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(testKeyValue))));

            //Act
            var cacheService = new CacheProviderService(_distributeCache.Object);
            var cachedResource = await cacheService.GetFromCache<TrafficStatistics>(testKey);

            //Assert
            Assert.NotNull(cachedResource);
            Assert.Equal(JsonConvert.SerializeObject(testKeyValue), JsonConvert.SerializeObject(cachedResource));
        }

        [Fact]
        public async Task SetCache_ShouldThrowException_IfKeyIsNull()
        {
            string key = null;

            var cacheService = new CacheProviderService(_distributeCache.Object);

            await Assert.ThrowsAsync<ArgumentNullException>(() =>cacheService.SetCache<string>(key, "Some str val", null));
        }

        private TrafficStatistics GetFakeTrafficStats()
        {
            return new TrafficStatistics()
            {
                BotIdentifier = Guid.NewGuid(),
                TrafficFlowRate = 100,
                AverageVehicleSpeed = 60,
                Longitude = 0,
                Latitude = 0,
                Altitude = 0,
                RoadName = "St Andrews Road",
                TrafficFlowDirection = Enums.Direction.South,
                TimeStamp = DateTime.Now
            };
        }
    }
}
