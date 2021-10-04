using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EagleRockHub.Models
{
    public class RedisOptions
    {
        public const string RedisConfig = "RedisConfig";

        public string HostName { get; set; }
        public string Port { get; set; }
    }
}
