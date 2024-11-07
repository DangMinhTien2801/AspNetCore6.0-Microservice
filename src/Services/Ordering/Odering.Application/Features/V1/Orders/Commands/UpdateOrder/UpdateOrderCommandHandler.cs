using AutoMapper;
using MediatR;
using Serilog;
using Odering.Application.Common.Interfaces;
using Odering.Application.Common.Models;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odering.Application.Common.Exceptions;

namespace Odering.Application.Features.V1.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler
        : IRequestHandler<UpdateOrderCommand, ApiResult<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository,
            IMapper mapper,
            ILogger logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        private const string MethodName = "UpdateOrderCommandHandler";
        public async Task<ApiResult<OrderDto>> Handle(UpdateOrderCommand request, 
            CancellationToken cancellationToken)
        {
            var orderEntity = await _orderRepository.GetByIdAsync(request.Id);
            if(orderEntity is null) 
                throw new NotFoundException(nameof(orderEntity), request.Id);
            _logger.Information($"BEGIN: {MethodName} - Order: {request.Id}");
            orderEntity = _mapper.Map(request, orderEntity);
            var updateOrder = await _orderRepository.UpdateOrderAsync(orderEntity);
            await _orderRepository.SaveChangeAsync();
            _logger.Information($"Order {request.Id} was successfully updated.");
            var result = _mapper.Map<OrderDto>(updateOrder);

            _logger.Information($"END: {MethodName} - Order: {request.Id}");
            return new ApiSuccessResult<OrderDto>(result);
        }
    }
}
