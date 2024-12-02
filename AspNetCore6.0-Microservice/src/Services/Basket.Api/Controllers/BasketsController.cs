using AutoMapper;
using Basket.Api.Entities;
using Basket.Api.Repositories.Interfaces;
using Basket.Api.Services;
using Basket.Api.Services.Interfaces;
using EventBus.Messages.IntegrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Serilog;
using Shared.DTOs.Basket;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Basket.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;
        private readonly BackgroundJobHttpService _backgroundJobHttp;
        private readonly IEmailTemplateService _emailTemplateService;

        public BasketsController(IBasketRepository basketRepository,
            IPublishEndpoint publishEndpoint,
            IMapper mapper,
            Serilog.ILogger logger,
            BackgroundJobHttpService backgroundJobHttp,
            IEmailTemplateService emailTemplateService)
        {
            _basketRepository = basketRepository;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
            _logger = logger;
            _backgroundJobHttp = backgroundJobHttp;
            _emailTemplateService = emailTemplateService;
        }
        [HttpGet("{username}", Name = "GetBasket")]
        [ProducesResponseType(typeof(CartDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBasketByUserName([Required]string username)
        {
            var cart = await _basketRepository.GetBasketByUserName(username);
            var result = _mapper.Map<CartDto>(cart) ?? new CartDto(username);

            return Ok(result);
        }

        [HttpPost(Name = "UpdateBasket")]
        [ProducesResponseType(typeof(CartDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateBasket([FromBody] CartDto model)
        {
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTimeOffset.UtcNow.AddHours(1))
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            var cart = _mapper.Map<Cart>(model);
            var updateCart = await _basketRepository.UpdateBasket(cart, options);
            var result = _mapper.Map<CartDto>(updateCart);
            return Ok(result);
        }

        [HttpDelete("{username}", Name = "Delete Basket")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            var result = await _basketRepository.DeleteBasketFromUserName(username);
            if(!result)
                return BadRequest(result);
            return Ok(result);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await _basketRepository
                .GetBasketByUserName(basketCheckout.UserName);
            if(basket == null) return NotFound();

            var evenMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            await _publishEndpoint.Publish(evenMessage);
            evenMessage.TotalPrice = basket.TotalPrice;

            await _basketRepository.DeleteBasketFromUserName(basketCheckout.UserName);
            return Accepted();
        }
    }
}
