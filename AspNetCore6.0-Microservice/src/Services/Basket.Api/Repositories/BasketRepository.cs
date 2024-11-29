using Basket.Api.Entities;
using Basket.Api.Repositories.Interfaces;
using Basket.Api.Services;
using Basket.Api.Services.Interfaces;
using Contracts.Common.Interfaces;
using Infrastructure.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Shared.DTOs.ScheduledJob;
using System.Text.Json;
using ILogger = Serilog.ILogger;

namespace Basket.Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCacheService;
        private readonly ISerializeService _serializeService;
        private readonly ILogger _logger;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly BackgroundJobHttpService _backgroundJobHttp;

        public BasketRepository(IDistributedCache redisCacheService,
            ISerializeService serializeService,
            ILogger logger,
            IEmailTemplateService emailTemplateService,
            BackgroundJobHttpService backgroundJobHttp)
        {
            _redisCacheService = redisCacheService;
            _serializeService = serializeService;
            _logger = logger;
            _emailTemplateService = emailTemplateService;
            _backgroundJobHttp = backgroundJobHttp;
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
            try
            {
                await TriggerSendEmailReminderCheckout(cart);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return await GetBasketByUserName(cart.UserName);
        }

        private async Task TriggerSendEmailReminderCheckout(Cart cart)
        {
            var emailTemplate = _emailTemplateService.GenerateReminderCheckoutOrderEmail(cart.UserName);

            var model = new ReminderCheckoutOrderDto(cart.EmailAddress, "Reminder checkout", emailTemplate, DateTimeOffset.UtcNow.AddSeconds(30));

            var uri = $"{_backgroundJobHttp.ScheduledJobUrl}/send-email-reminder-checkout-order";
            var response = await _backgroundJobHttp.Client.PostAsJson(uri, model);
            if (response.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                var jobId = await response.ReadContentAs<string>();
                if (!string.IsNullOrEmpty(jobId))
                {
                    cart.JobId = jobId;
                    await _redisCacheService.SetStringAsync(cart.UserName,
                        _serializeService.Serialize(cart));
                }
            }
        }

        private async Task DeleteReminderCheckoutOrder(string username)
        {
            var cart = await GetBasketByUserName(username);
            if (cart == null || string.IsNullOrEmpty(cart.JobId)) return;

            var jobId = cart.JobId;
            var uri = $"{_backgroundJobHttp.ScheduledJobUrl}/delete/jobId/{jobId}";
            await _backgroundJobHttp.Client.DeleteAsync(uri);
            _logger.Information($"DeleteReminderCheckoutOrder: Deleted JobId: {jobId}");
        }

        public async Task<bool> DeleteBasketFromUserName(string userName)
        {
            await DeleteReminderCheckoutOrder(userName);
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
