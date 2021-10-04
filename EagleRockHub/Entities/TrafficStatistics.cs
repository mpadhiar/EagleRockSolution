using EagleRockHub.Enums;
using EagleRockHub.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EagleRockHub.Entities
{
    public class TrafficStatistics : IEagleBotLocation
    {
        public Guid BotIdentifier { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Altitude { get; set; }
        public DateTime TimeStamp { get; set; }
        public int TrafficFlowRate { get; set; }
        public Direction TrafficFlowDirection { get; set; }
        public string RoadName { get; set; }
        public double AverageVehicleSpeed { get; set; }
    }
}
