using AutoMapper;
using MediatR;
using Odering.Application.Common.Interfaces;
using Odering.Application.Common.Models;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odering.Application.Features.V1.Orders.Queries
{
    public class GetOrderQueryHandler
        : IRequestHandler<GetOrderQuery, ApiResult<List<OrderDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;

        public GetOrderQueryHandler(IMapper mapper, IOrderRepository repository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        public async Task<ApiResult<List<OrderDto>>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var orderEntities = await _repository.GetOrderByUserName(request.UserName);
            var orderList = _mapper.Map<List<OrderDto>>(orderEntities); 

            return new ApiSuccessResult<List<OrderDto>>(orderList);
        }
    }
}
