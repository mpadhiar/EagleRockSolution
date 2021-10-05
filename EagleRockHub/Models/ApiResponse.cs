using EagleRockHub.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EagleRockHub.Models
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            Messages = new List<ApiMessage>();
        }

        public List<ApiMessage> Messages { get; set; }

        public bool Success
        {
            get
            {
                return !Messages.Any(x =>
                    x.MessageType == ApiMessageType.Exception ||
                    x.MessageType == ApiMessageType.Error);
            }
        }
        
        [JsonIgnore]
        public bool HasExceptions
        {
            get
            {
                return Messages.Any(x => x.MessageType == ApiMessageType.Exception);
            }
        }

        public void AddMessage(string message, ApiMessageType apiMessageType = ApiMessageType.Information)
        {
            if(Messages == null)
            {
                return;
            }

            if(string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException(nameof(message), "The message cannot be null or empty");
            }

            Messages.Add(new ApiMessage()
            {
                Message = message,
                MessageType = apiMessageType
            });
        }
    }
}
