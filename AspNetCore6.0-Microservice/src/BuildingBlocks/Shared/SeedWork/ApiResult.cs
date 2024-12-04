using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.SeedWork
{
    public class ApiResult<T>
    {
        public string Message { get; set; } = null!;
        public bool IsSucceeded { get; set; }
        public T? Data { get; set; }
        public ApiResult()
        {
        }
        [JsonConstructor]
        public ApiResult(bool isSucceeded, string? message = null)
        {
            Message = message ?? string.Empty;
            IsSucceeded = isSucceeded;
        }

        public ApiResult(bool isSucceeded, T data, string? message = null)
        {
            Data = data;
            IsSucceeded = isSucceeded;
            Message = message ?? string.Empty;
        }
    }
}
