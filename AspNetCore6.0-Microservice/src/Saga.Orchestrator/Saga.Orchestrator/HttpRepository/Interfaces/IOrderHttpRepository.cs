using Shared.DTOs.Order;

namespace Saga.Orchestrator.HttpRepository.Interfaces
{
    public interface IOrderHttpRepository
    {
        Task<string?> CreateOrder(CreateOrderDto orrder);
        Task<OrderDto> GetOrder(string id);
        Task<bool> DeleteOrder(string id);
        Task<bool> DeleteOrderByDocumentNo(string documentNo);
    }
}
