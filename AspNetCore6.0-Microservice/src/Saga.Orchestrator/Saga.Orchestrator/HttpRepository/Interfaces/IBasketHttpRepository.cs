using Shared.DTOs.Basket;

namespace Saga.Orchestrator.HttpRepository.Interfaces
{
    public interface IBasketHttpRepository
    {
        Task<CartDto> GetBasket(string userName);
        Task<bool> DeleteBasket(string userName);
    }
}
