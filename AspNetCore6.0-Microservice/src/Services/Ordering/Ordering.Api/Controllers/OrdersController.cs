using AutoMapper;
using Contracts.Messages;
using Contracts.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Odering.Application.Common.Interfaces;
using Odering.Application.Common.Models;
using Odering.Application.Features.V1.Orders.Queries;
using Ordering.Domain.Entities;
using Shared.Services.Email;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Ordering.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ISmtpEmailService _emailService;
        private readonly IMessageProducer _messageProducer;
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        public OrdersController(IMediator mediator, 
            ISmtpEmailService emailService,
            IMessageProducer messageProducer,
            IOrderRepository repository,
            IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _messageProducer = messageProducer ?? throw new ArgumentNullException(nameof(messageProducer));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        private static class RouteNames
        {
            public const string GetOrders = nameof(GetOrders);
        }
        [HttpGet("{username}", Name = RouteNames.GetOrders)]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrderByUserName([Required] string username)
        {
            var query = new GetOrderQuery(username);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> TestEmail()
        {
            var message = new MailRequest
            {
                Body = "<h1>Welcome</h1>",
                Subject = "Test",
                ToAddress = "dtien5333@gmail.com"
            };
            await _emailService.SendEmailAsync(message);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody]OrderDto orderDto)
        {
            orderDto.Id = Guid.NewGuid().ToString();
            var order = _mapper.Map<Order>(orderDto);
            var addedOrder = await _repository.CreateOrder(order);
            var result = _mapper.Map<OrderDto>(addedOrder);
            _messageProducer.SendMessage(result);
            return Ok(result);
        }
        
    }
}
