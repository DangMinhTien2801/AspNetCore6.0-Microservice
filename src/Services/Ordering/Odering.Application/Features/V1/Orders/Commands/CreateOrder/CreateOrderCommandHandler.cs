using AutoMapper;
using Contracts.Services;
using MediatR;
using Serilog;
using Odering.Application.Common.Interfaces;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ordering.Domain.Entities;
using Shared.Services.Email;

namespace Odering.Application.Features.V1.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler
        : IRequestHandler<CreateOrderCommand, ApiResult<string>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ISmtpEmailService _emailService;
        private readonly ILogger _logger;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            IMapper mapper,
            ISmtpEmailService emailService,
            ILogger logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        private const string MethodName = "CreateOrderCommandHandler";
        public async Task<ApiResult<string>> Handle(CreateOrderCommand request, 
            CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: {MethodName} - UserName: {request.UserName}");
            var orderEntity = _mapper.Map<Order>(request);
            orderEntity.Id = Guid.NewGuid().ToString();
            var addedOrder = await _orderRepository.CreateOrder(orderEntity);
            await _orderRepository.SaveChangeAsync();
            _logger.Information($"Order {addedOrder.Id} is successfully created.");

            await SendEmailAsync(addedOrder, cancellationToken);
            _logger.Information($"END: {MethodName} - UserName: {request.UserName}");

            return new ApiSuccessResult<string>(addedOrder.Id);
        }
        public async Task SendEmailAsync(Order order, 
            CancellationToken cancellationToken)
        {
            var emailRequest = new MailRequest
            {
                ToAddress = order.EmailAddress,
                Body = "Order was created",
                Subject = "Order was created"
            };
            try
            {
                await _emailService.SendEmailAsync(emailRequest, cancellationToken);
                _logger.Information($"Sent Created Order to email {order.EmailAddress}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Order {order.Id} failed due to an error with the email service: {ex.Message}");
            }
        }
    }
}
