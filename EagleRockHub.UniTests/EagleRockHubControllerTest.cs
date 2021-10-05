using EagleRockHub.Controllers;
using EagleRockHub.Dtos;
using EagleRockHub.Enums;
using EagleRockHub.Interfaces;
using EagleRockHub.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace EagleRockHub.UniTests
{
    public class EagleRockHubControllerTest
    {

        private readonly Mock<IEagleHubService> _eagleHubService;
        private readonly Mock<ILogger<EagleRockHubController>> _logger;

        public EagleRockHubControllerTest()
        {
            _eagleHubService = new Mock<IEagleHubService>();
            _logger = new Mock<ILogger<EagleRockHubController>>();
        }

        [Fact]
        public async Task AddTrafficStats_ReturnsSuccess_WhenAValidItemIsAdded()
        {
            //Arrange
            var fakeTrafficStats = GetFakeTrafficStats();

            _eagleHubService.Setup(x => x.AddTrafficStatsAsync(fakeTrafficStats))
                .Returns(Task.FromResult(new ApiResponse()));

            //Act
            var eagleHubController = new EagleRockHubController(_logger.Object, _eagleHubService.Object);
            var actionResult = await eagleHubController.AddTrafficStatsAsync(fakeTrafficStats);

            //Assert
            var objectResult = Assert.IsType<OkObjectResult>(actionResult);
            var apiResponse = Assert.IsType<ApiResponse>(objectResult.Value);
            Assert.True(apiResponse.Success);
            Assert.False(apiResponse.HasExceptions);
        }

        [Fact]
        public async Task AddTrafficStats_ReturnsBadRequest_WhenApiResponseHasValidationErrors()
        {
            //Arrange
            var fakeTrafficStats = GetFakeTrafficStats();
            var fakeApiResponse = new ApiResponse();
            fakeApiResponse.AddMessage("The guid identifier for the eagle bot is missing", ApiMessageType.Error);

            _eagleHubService.Setup(x => x.AddTrafficStatsAsync(fakeTrafficStats))
                .Returns(Task.FromResult(fakeApiResponse));
            //Act
            var eagleHubController = new EagleRockHubController(_logger.Object, _eagleHubService.Object);
            var actionResult = await eagleHubController.AddTrafficStatsAsync(fakeTrafficStats);

            //Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(actionResult);
            var apiResponse = Assert.IsType<ApiResponse>(objectResult.Value);
            Assert.False(apiResponse.Success);
            Assert.Collection(apiResponse.Messages, x => x.Message.Equals(fakeApiResponse.Messages[0].Message));
        }

        [Fact]
        public async Task AddTrafficStats_ReturnsInternalServerError_WhenApiResponseHasExceptions()
        {
            //Arrange
            var fakeTrafficStats = GetFakeTrafficStats();
            var fakeApiResponse = new ApiResponse();
            fakeApiResponse.AddMessage("An exception occured when running the api", ApiMessageType.Exception);

            _eagleHubService.Setup(x => x.AddTrafficStatsAsync(fakeTrafficStats))
                .Returns(Task.FromResult(fakeApiResponse));
            //Act
            var eagleHubController = new EagleRockHubController(_logger.Object, _eagleHubService.Object);
            var actionResult = await eagleHubController.AddTrafficStatsAsync(fakeTrafficStats);

            //Assert
            var objectResult = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(objectResult.StatusCode, StatusCodes.Status500InternalServerError);
            var apiResponse = Assert.IsType<ApiResponse>(objectResult.Value);
            Assert.False(apiResponse.Success);
            Assert.True(apiResponse.HasExceptions);
            Assert.Collection(apiResponse.Messages, x => x.Message.Equals(fakeApiResponse.Messages[0].Message));
        }

        [Fact]
        public async Task GetTrafficStats_ReturnsTrafficInformationList_WhenInvoked()
        {
            //Arrange
            var fakeApiResponse = new ApiDataResponse<List<TrafficStatisticsDto>>();
            fakeApiResponse.Data.Add(GetFakeTrafficStats());
            fakeApiResponse.Data.Add(GetFakeTrafficStats());

            _eagleHubService.Setup(x => x.GetAllTrafficStatsAsync())
                .Returns(Task.FromResult(fakeApiResponse));
            //Act
            var eagleHubController = new EagleRockHubController(_logger.Object, _eagleHubService.Object);
            var actionResult = await eagleHubController.GetAllTrafficStatsAsync();

            //Assert
            var objectResult = Assert.IsType<OkObjectResult>(actionResult);
            var apiResponse = Assert.IsType<ApiDataResponse<List<TrafficStatisticsDto>>>(objectResult.Value);
            Assert.True(apiResponse.Success);
            Assert.False(apiResponse.HasExceptions);
            Assert.NotNull(apiResponse.Data);
            Assert.Equal(apiResponse.Data.Count, fakeApiResponse.Data.Count);
        }

        private TrafficStatisticsDto GetFakeTrafficStats()
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
