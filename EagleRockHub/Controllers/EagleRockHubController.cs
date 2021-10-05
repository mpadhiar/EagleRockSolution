using EagleRockHub.Attributes;
using EagleRockHub.Dtos;
using EagleRockHub.Entities;
using EagleRockHub.Interfaces;
using EagleRockHub.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EagleRockHub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EagleRockHubController : ControllerBase
    {
        private readonly ILogger<EagleRockHubController> _logger;
        private readonly IEagleHubService _eagleHubService;

        public EagleRockHubController(ILogger<EagleRockHubController> logger, IEagleHubService eagleHubService)
        {
            _logger = logger;
            _eagleHubService = eagleHubService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiDataResponse<List<TrafficStatisticsDto>>), 200)]
        public async Task<IActionResult> GetAllTrafficStatsAsync()
        {
            var stats = await _eagleHubService.GetAllTrafficStatsAsync();
            return Ok(stats);
        }

        [HttpPost]
        [ValidateParam]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        public async Task<IActionResult> AddTrafficStatsAsync(TrafficStatisticsDto trafficStatisticsDto)
        {
            var response = await _eagleHubService.AddTrafficStatsAsync(trafficStatisticsDto);

            if(!response.Success)
            {
                if (response.HasExceptions)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
