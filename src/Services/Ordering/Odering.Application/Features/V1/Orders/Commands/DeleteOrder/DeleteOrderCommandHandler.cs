using MediatR;
using Serilog;
using Odering.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odering.Application.Common.Exceptions;

namespace Odering.Application.Features.V1.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler
        : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger _logger;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository,
            ILogger logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = await _orderRepository.GetByIdAsync(request.Id);
            if(orderEntity == null)
            {
                throw new NotFoundException(nameof(orderEntity), request.Id);
            }
            await _orderRepository.DeleteAsync(orderEntity);
            await _orderRepository.SaveChangeAsync();

            _logger.Information($"Order {orderEntity.Id} was successfully deleted.");

            return Unit.Value;
        }
    }
}
