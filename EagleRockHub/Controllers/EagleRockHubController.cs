using EagleRockHub.Entities;
using EagleRockHub.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EagleRockHub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EagleRockHubController : ControllerBase
    {
        private readonly ILogger<EagleRockHubController> _logger;
        private readonly IEagleHubRepository _eagleHubRepository;

        public EagleRockHubController(ILogger<EagleRockHubController> logger, IEagleHubRepository eagleHubRepository)
        {
            _logger = logger;
            _eagleHubRepository = eagleHubRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<TrafficStatistics>), 200)]
        public async Task<IActionResult> TrafficStatsAsync()
        {
            var stats = await _eagleHubRepository.GetTrafficStatistics();
            return Ok(stats);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> TrafficStatsAsync(TrafficStatistics trafficStatistics)
        {
            await _eagleHubRepository.AddTrafficStatistics(trafficStatistics);
            return Ok();
        }
    }
}
