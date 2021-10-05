using AutoMapper;
using EagleRockHub.Dtos;
using EagleRockHub.Entities;
using EagleRockHub.Enums;
using EagleRockHub.Interfaces;
using EagleRockHub.Models;
using EagleRockHub.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EagleRockHub.Services
{
    public class EagleHubService : IEagleHubService
    {
        private readonly IEagleHubRepository _eagleHubRepository;
        private readonly ILogger<EagleHubService> _logger;
        private readonly IMapper _mapper;

        public EagleHubService(IEagleHubRepository eagleHubRepository, ILogger<EagleHubService> logger, IMapper mapper)
        {
            _eagleHubRepository = eagleHubRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ApiDataResponse<List<TrafficStatisticsDto>>> GetAllTrafficStatistics()
        {
            var apiResponse = new ApiDataResponse<List<TrafficStatisticsDto>>();

            var trafficStats = _eagleHubRepository.GetTrafficStatistics();

            //if we dont have any stats in cache we return an empty list
            if(trafficStats == null)
            {
                apiResponse.Data = new List<TrafficStatisticsDto>();
                return apiResponse;
            }


        }

        public async Task<ApiResponse> AddTrafficStatistics(TrafficStatisticsDto trafficStatisticDto)
        {
            var apiResponse = new ApiResponse();
      
            /*We are only validating that the bot identifiers are not empty here. Ideally we should have a list of valid bot idenfifiers in the data store 
             * and this should also check if the identifer for the bot is valid.
             */
            if (trafficStatisticDto.BotIdentifier == Guid.Empty)
            {
                apiResponse.AddMessage("A valid eagle bot identifier is requried to add traffic information", ApiMessageType.Error);
                return apiResponse;
            }
            try
            {
                var entity = _mapper.Map<TrafficStatistics>(trafficStatisticDto);
                await _eagleHubRepository.AddTrafficStatistics(entity);
            }
            catch(Exception ex)
            {
                //logs any errors to the debug console. Ideally we should use app insights or serilog to log any error
                _logger.LogError(ex, ex.Message);
                apiResponse.AddMessage(ex.Message, ApiMessageType.Exception);
            }

            _logger.LogInformation($"Successfully added traffice stats received from bot with identifier {trafficStatisticDto.BotIdentifier}");
            return apiResponse;
        }

    }
}
