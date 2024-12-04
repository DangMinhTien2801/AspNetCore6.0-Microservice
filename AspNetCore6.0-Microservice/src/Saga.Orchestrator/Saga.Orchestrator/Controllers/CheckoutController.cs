using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saga.Orchestrator.Services.Interfaces;
using Shared.DTOs.Basket;

namespace Saga.Orchestrator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutSagaService _checkoutSagaService;

        public CheckoutController(ICheckoutSagaService checkoutSagaService)
        {
            _checkoutSagaService = checkoutSagaService;
        }

        [HttpPost("{username}")]
        public async Task<IActionResult> CheckoutOrder(string username, 
            BasketCheckoutDto model)
        {
            var result = await _checkoutSagaService.CheckoutOrder(username, model);
            return Accepted(result);
        }
    }
}
