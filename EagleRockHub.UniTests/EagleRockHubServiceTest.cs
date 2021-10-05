using AutoMapper;
using EagleRockHub.Dtos;
using EagleRockHub.Entities;
using EagleRockHub.Interfaces;
using EagleRockHub.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EagleRockHub.UniTests
{
    public class EagleRockHubServiceTest
    {

        private readonly Mock<IEagleHubRepository> _eagleHubRepository;
        private readonly Mock<ILogger<EagleHubService>> _logger;
        private readonly Mock<IMapper> _mapper;

        public EagleRockHubServiceTest()
        {
            _eagleHubRepository = new Mock<IEagleHubRepository>();
            _logger = new Mock<ILogger<EagleHubService>>();
            _mapper = new Mock<IMapper>();
            InitializeAutoMapperMock();
        }

        [Fact]
        public async Task GetAllTrafficStats_ShouldReturnEmptyList_WhenRepositoryHasNoValues()
        {
            _eagleHubRepository.Setup(x => x.GetTrafficStatisticsAsync())
                .Returns(Task.FromResult<List<TrafficStatistics>>(null));

            //Act
            var eagleHubService = new EagleHubService(_eagleHubRepository.Object, _logger.Object, _mapper.Object);
            var apiResponse = await eagleHubService.GetAllTrafficStatsAsync();

            //Assert
            Assert.NotNull(apiResponse);
            Assert.NotNull(apiResponse.Data);
            Assert.Empty(apiResponse.Data);
            Assert.True(apiResponse.Success);
        }

        [Fact]
        public async Task GetAllTrafficStats_ShouldReturnList_WhenRepositoryHasValues()
        {
            List<TrafficStatistics> trafficStatictics = new()
            {
                GetFakeTrafficStats(),
                GetFakeTrafficStats()
            };

            _eagleHubRepository.Setup(x => x.GetTrafficStatisticsAsync())
                .Returns(Task.FromResult<List<TrafficStatistics>>(trafficStatictics));

            //Act
            var eagleHubService = new EagleHubService(_eagleHubRepository.Object, _logger.Object, _mapper.Object);
            var apiResponse = await eagleHubService.GetAllTrafficStatsAsync();

            //Assert
            Assert.NotNull(apiResponse);
            Assert.NotNull(apiResponse.Data);
            Assert.True(apiResponse.Success);

            Assert.Equal(apiResponse.Data.Count, trafficStatictics.Count);

            Assert.Equal(apiResponse.Data[0].RoadName, trafficStatictics[0].RoadName);
            Assert.Equal(apiResponse.Data[0].TimeStamp, trafficStatictics[0].TimeStamp);
            Assert.Equal(apiResponse.Data[0].Altitude, trafficStatictics[0].Altitude);
            Assert.Equal(apiResponse.Data[0].Latitude, trafficStatictics[0].Latitude);
            Assert.Equal(apiResponse.Data[0].Longitude, trafficStatictics[0].Longitude);
        }

        [Fact]
        public async Task AddTrafficStats_ShouldReturnFailureResponse_WhenBoatIdentifierIsEmpty()
        {
            var fakeTrafficStats = GetFakeTrafficStatsDto();
            fakeTrafficStats.BotIdentifier = Guid.Empty;

            //Act
            var eagleHubService = new EagleHubService(_eagleHubRepository.Object, _logger.Object, _mapper.Object);
            var apiResponse = await eagleHubService.AddTrafficStatsAsync(fakeTrafficStats);

            //Assert
            Assert.NotNull(apiResponse);
            Assert.False(apiResponse.Success);
            Assert.Single(apiResponse.Messages);
        }

        [Fact]
        public async Task AddTrafficStats_ShouldReturnSuccess_WhenDtoIsValid()
        {
            var fakeTrafficStats = GetFakeTrafficStatsDto();

            //Act
            var eagleHubService = new EagleHubService(_eagleHubRepository.Object, _logger.Object, _mapper.Object);
            var apiResponse = await eagleHubService.AddTrafficStatsAsync(fakeTrafficStats);

            //Assert
            Assert.NotNull(apiResponse);
            Assert.True(apiResponse.Success);
            Assert.Empty(apiResponse.Messages);
        }

        private void InitializeAutoMapperMock()
        {
            _mapper.Setup(x => x.Map<List<TrafficStatisticsDto>>(It.IsAny<List<TrafficStatistics>>()))
                        .Returns((List<TrafficStatistics> sourceList) =>
                        {
                            return sourceList.Select(source =>
                                   new TrafficStatisticsDto
                                   {
                                       AverageVehicleSpeed = source.AverageVehicleSpeed,
                                       Longitude = source.Longitude,
                                       Latitude = source.Latitude,
                                       Altitude = source.Altitude,
                                       TimeStamp = source.TimeStamp,
                                       TrafficFlowRate = source.TrafficFlowRate,
                                       RoadName = source.RoadName
                                   }).ToList();
                        });

            _mapper.Setup(x => x.Map<TrafficStatistics>(It.IsAny<TrafficStatisticsDto>()))
                        .Returns((TrafficStatisticsDto source) =>
                        {
                            return new TrafficStatistics
                            {
                                AverageVehicleSpeed = source.AverageVehicleSpeed,
                                Longitude = source.Longitude,
                                Latitude = source.Latitude,
                                Altitude = source.Altitude,
                                TimeStamp = source.TimeStamp,
                                TrafficFlowRate = source.TrafficFlowRate,
                                RoadName = source.RoadName
                            };

                        });

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

        private TrafficStatisticsDto GetFakeTrafficStatsDto()
        {
            return new TrafficStatisticsDto()
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
