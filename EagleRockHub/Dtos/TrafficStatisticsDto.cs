using EagleRockHub.Enums;
using EagleRockHub.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EagleRockHub.Dtos
{
    public class TrafficStatisticsDto : IEagleBotLocation
    {
        public Guid? BotIdentifier { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Altitude { get; set; }
        public DateTime TimeStamp { get; set; }
        public int TrafficFlowRate { get; set; }
        public Direction TrafficFlowDirection { private get; set; }
        public string FlowDirection => TrafficFlowDirection.ToString();
        [Required]
        public string RoadName { get; set; }
        public double AverageVehicleSpeed { get; set; }
    }
}
