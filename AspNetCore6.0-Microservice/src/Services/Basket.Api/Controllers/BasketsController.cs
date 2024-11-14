using AutoMapper;
using Basket.Api.Entities;
using Basket.Api.Repositories.Interfaces;
using EventBus.Messages.IntegrationEvents.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Basket.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public BasketsController(IBasketRepository basketRepository,
            IPublishEndpoint publishEndpoint,
            IMapper mapper)
        {
            _basketRepository = basketRepository;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
        }
        [HttpGet("{username}", Name = "GetBasket")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBasketByUserName([Required]string username)
        {
            var result = await _basketRepository.GetBasketByUserName(username);
            return Ok(result ?? new Cart());
        }

        [HttpPost(Name = "UpdateBasket")]
        [ProducesResponseType(typeof(Cart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateBasket([FromBody] Cart cart)
        {
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTimeOffset.UtcNow.AddHours(1))
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            var result = await _basketRepository.UpdateBasket(cart, options);
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
