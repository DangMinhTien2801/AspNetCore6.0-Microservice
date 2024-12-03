using Infrastructure.Extensions;
using Saga.Orchestrator.HttpRepository.Interfaces;
using Shared.DTOs.Order;
using Shared.SeedWork;
using System.Net.Http;

namespace Saga.Orchestrator.HttpRepository
{
    public class OrderHttpRepository : IOrderHttpRepository
    {
        private readonly HttpClient _client;
        public OrderHttpRepository(HttpClient client)
        {
            _client = client;
        }
        public async Task<string?> CreateOrder(CreateOrderDto orrder)
        {
            var response = await _client.PostAsJsonAsync("orders", orrder);
            if (!response.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                return null;
            }

            var orderId = await response.ReadContentAs<ApiSuccessResult<string>>();
            return orderId?.Data;
        }

        public async Task<bool> DeleteOrder(string id)
        {
            var response = await _client.DeleteAsync($"orders/{id.ToString()}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteOrderByDocumentNo(string documentNo)
        {
            var response = await _client.DeleteAsync($"orders/document-no/{documentNo}");
            return response.IsSuccessStatusCode;
        }

        public async Task<OrderDto> GetOrder(string id)
        {
            var order = await _client.GetFromJsonAsync<ApiSuccessResult<OrderDto>>($"orders/GetOrderById/{id}");
            return order.Data;
        }
    }
}
