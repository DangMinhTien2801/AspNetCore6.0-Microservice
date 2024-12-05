using Contracts.Sagas.OrderManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saga.Orchestrator.OrderManager;
using Saga.Orchestrator.Services.Interfaces;
using Shared.DTOs.Basket;

namespace Saga.Orchestrator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ISagaOrderManager<BasketCheckoutDto, OrderResponse> _sageOrderManager;

        public CheckoutController(ISagaOrderManager<BasketCheckoutDto, OrderResponse> SageOrderManager)
        {
            _sageOrderManager = SageOrderManager;
        }

        [HttpPost("{username}")]
        public OrderResponse CheckoutOrder(string username, 
            BasketCheckoutDto model)
        {
            model.UserName = username;
            var result = _sageOrderManager.CreateOrder(model);
            return result;
        }
    }
}
