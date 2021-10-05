using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EagleRockHub.Models
{
    public class ApiDataResponse<T> : ApiResponse where T : new()
    {
        public ApiDataResponse()
        {
            Data = new T();
        }

        public T Data { get; set; }
    }
}
