using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.SeedWork
{
    public class ApiSuccessResult<T> : ApiResult<T>
    {
        [JsonConstructor]
        public ApiSuccessResult(T data)
            : base(true, data, "Success")
        {            
        }
        public ApiSuccessResult(T data, string message)
            : base(true, data, message)
        {
        }
    }
}
