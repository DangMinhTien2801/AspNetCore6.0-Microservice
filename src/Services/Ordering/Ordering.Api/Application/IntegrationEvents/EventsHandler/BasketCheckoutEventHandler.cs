using AutoMapper;
using EventBus.Messages.IntegrationEvents.Events;
using MassTransit;
using MediatR;
using Odering.Application.Features.V1.Orders.Commands.CreateOrder;
using ILogger = Serilog.ILogger;

namespace Ordering.Api.Application.IntegrationEvents.EventsHandler
{
    public class BasketCheckoutEventHandler
        : IConsumer<BasketCheckoutEvent>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public BasketCheckoutEventHandler(IMediator mediator, IMapper mapper, ILogger logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var command = _mapper.Map<CreateOrderCommand>(context.Message);
            var result = await _mediator.Send(command);
            _logger.Information("BasketCheckoutEvent consumed successfully. " + 
                "Order is created with Id: " + result.Data);
        }
    }
}
