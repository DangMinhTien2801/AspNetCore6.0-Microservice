using AutoMapper;
using DnsClient.Internal;
using MediatR;
using Odering.Application.Common.Interfaces;
using Odering.Application.Common.Models;
using Serilog.Sinks.SystemConsole.Themes;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odering.Application.Features.V1.Orders.Queries.GetOrderById
{
    public class GetOrderByQueryIdHandler :
        IRequestHandler<GetOrderByIdQuery, ApiResult<OrderDto>>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;
        private readonly Serilog.ILogger _logger;
        public GetOrderByQueryIdHandler(IMapper mapper, 
            IOrderRepository repository, 
            Serilog.ILogger logger)
        {
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _repository = repository ?? throw new ArgumentException(nameof(_repository));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        private const string MethodName = "GetOrderByIdQueryHandler";

        public async Task<ApiResult<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: {MethodName} - Id: {request.Id}");

            var order = await _repository.GetByIdAsync(request.Id);
            var orderDto = _mapper.Map<OrderDto>(order);
            
            _logger.Information($"END: {MethodName} - Id: {request.Id}");

            return new ApiSuccessResult<OrderDto>(orderDto);    
        }
    }
}
