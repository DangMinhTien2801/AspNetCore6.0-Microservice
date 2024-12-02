using MediatR;
using Odering.Application.Common.Models;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odering.Application.Features.V1.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQuery : IRequest<ApiResult<OrderDto>>
    {
        public string Id { get; set; }
        public GetOrderByIdQuery(string id)
        {
            Id = id;
        }
    }
}
