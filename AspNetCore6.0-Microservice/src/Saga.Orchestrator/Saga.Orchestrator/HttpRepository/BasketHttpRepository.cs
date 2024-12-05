using Saga.Orchestrator.HttpRepository.Interfaces;
using Shared.DTOs.Basket;

namespace Saga.Orchestrator.HttpRepository
{
    public class BasketHttpRepository : IBasketHttpRepository
    {
        private readonly HttpClient _client;
        public async Task<bool> DeleteBasket(string userName)
        {
            var response = await _client.DeleteAsync($"baskets/{userName}");
            if (!response.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                throw new Exception($"Delete basket for UserName No: {userName} not success");
            }

            var result = response.IsSuccessStatusCode;
            return result;
        }

        public async Task<CartDto> GetBasket(string userName)
        {
            var cart = await _client.GetFromJsonAsync<CartDto>($"basket/{userName}");
            if(cart == null || !cart.Items.Any()) return null;

            return cart;
        }
    }
}
