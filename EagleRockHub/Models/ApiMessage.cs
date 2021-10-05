using EagleRockHub.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EagleRockHub.Models
{
    public class ApiMessage
    {
        public ApiMessageType MessageType { get; set; }
        public string Message { get; set; }
    }
}
