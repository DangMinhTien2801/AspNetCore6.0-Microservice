using Basket.Api.Entities;
using Basket.Api.Repositories.Interfaces;
using Contracts.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using ILogger = Serilog.ILogger;

namespace Basket.Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCacheService;
        private readonly ISerializeService _serializeService;
        private readonly ILogger _logger;
        public BasketRepository(IDistributedCache redisCacheService,
            ISerializeService serializeService,
            ILogger logger)
        {
            _redisCacheService = redisCacheService;
            _serializeService = serializeService;
            _logger = logger;

        }
        public async Task<Cart?> GetBasketByUserName(string userName)
        {
            _logger.Information($"Begin GetBasketByUserName: {userName}");
            var basket = await _redisCacheService.GetStringAsync(userName);
            if(!string.IsNullOrWhiteSpace(basket))
            {
                var result = _serializeService.Deserialize<Cart?>(basket);
                var totalPrice = result == null ? 0 : result?.TotalPrice;
                _logger.Information("End GetBasketByUserName: {userName} " +
                    "- Total Price: {totalPrice}", userName, totalPrice);
                return result;
            }
            return null;
        }

        public async Task<Cart?> UpdateBasket(Cart cart, DistributedCacheEntryOptions? options = null)
        {
            _logger.Information($"Begin UpdateBasket: {cart.UserName}");
            if (options != null)
                await _redisCacheService.SetStringAsync(cart.UserName,
                    _serializeService.Serialize(cart), options);
            else
                await _redisCacheService.SetStringAsync(cart.UserName,
                    _serializeService.Serialize(cart));
            _logger.Information($"End UpdateBasket: {cart.UserName}");
            return await GetBasketByUserName(cart.UserName);
        }
        public async Task<bool> DeleteBasketFromUserName(string userName)
        {
            try
            {
                _logger.Information($"Begin DeleteBasketFromUserName: {userName}");
                await _redisCacheService.RemoveAsync(userName);
                _logger.Information($"End DeleteBasketFromUserName: {userName}");
                return true;
            }
            catch(Exception ex)
            {
                _logger.Error("DeleteBasketFromUserName: " + ex.Message);
                _logger.Information($"End DeleteBasketFromUserName: {userName}");
                return false;
            }
        }

    }
}
