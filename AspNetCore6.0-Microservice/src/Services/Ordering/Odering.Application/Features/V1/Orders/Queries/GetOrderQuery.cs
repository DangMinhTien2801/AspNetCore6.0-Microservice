using MediatR;
using Odering.Application.Common.Models;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odering.Application.Features.V1.Orders.Queries
{
    public class GetOrderQuery : IRequest<ApiResult<List<OrderDto>>>
    {
        public string UserName { get; private set; } = null!;
        public GetOrderQuery(string userName)
        {
            UserName = userName ?? 
                throw new ArgumentNullException(nameof(userName));
        }
    }
}
