using AutoMapper;
using Saga.Orchestrator.HttpRepository;
using Saga.Orchestrator.HttpRepository.Interfaces;
using Saga.Orchestrator.Services.Interfaces;
using Shared.DTOs.Basket;
using Shared.DTOs.Inventory;
using Shared.DTOs.Order;

namespace Saga.Orchestrator.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IOrderHttpRepository _orderHttpRepository;
        private readonly IBasketHttpRepository _basketHttpRepository;
        private readonly IInventoryHttpRepository _inventoryHttpRepository;
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;

        public CheckoutService(IOrderHttpRepository orderHttpRepository,
            IBasketHttpRepository basketHttpRepository,
            IInventoryHttpRepository inventoryHttpRepository,
            IMapper mapper,
            Serilog.ILogger logger)
        {
            _orderHttpRepository = orderHttpRepository;
            _basketHttpRepository = basketHttpRepository;
            _inventoryHttpRepository = inventoryHttpRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<bool> CheckoutOrder(string userName, BasketCheckoutDto basketCheckoutDto)
        {
            // Get cart from BasketHttpRepository
            _logger.Information($"Start: Get Cart {userName}");

            var cart = await _basketHttpRepository.GetBasket(userName);
            if(cart == null) return false;
            _logger.Information($"End: Get Cart {userName} success");

            // Create Order from OrderHttpRepository
            _logger.Information($"Start: Create Order");

            var order = _mapper.Map<CreateOrderDto>(basketCheckoutDto);
            order.TotalPrice = cart.TotalPrice;
            // Get Order by order id
            var orderId = await _orderHttpRepository.CreateOrder(order);
            if(orderId == null) return false;
            var addedOrder = await _orderHttpRepository.GetOrder(orderId);
            _logger.Information($"End: Created Order success, Order Id: {orderId} - Document No - {addedOrder.DocumentNo}");

            var inventoryDocumentNos = new List<string>();
            bool result;
            try
            {
                // Sales Items from InventoryHttpRepository
                foreach(var item in cart.Items)
                {
                    _logger.Information($"Start: Sale Item No: {item.ItemNo} - Quantity - {item.Quantity}");

                    var saleOrder = new SalesProductDto(addedOrder.DocumentNo, item.Quantity);
                    saleOrder.SetItemNo(item.ItemNo);
                    var documentNo = await _inventoryHttpRepository.CreateSalesOrder(saleOrder);
                    inventoryDocumentNos.Add(documentNo);

                    _logger.Information($"End: Sale Item No: {item.ItemNo} - Quantity - {item.Quantity} - Document No: {documentNo}");
                }

                result = await _basketHttpRepository.DeleteBasket(userName);
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message);
                // Roleback checkout order
                await RollbackCheckoutOrder(userName, addedOrder.Id, inventoryDocumentNos);
                throw;
            }

            return result;
        }

        private async Task RollbackCheckoutOrder(string userName, long orderId, List<string> inventoryDocumentNos)
        {

        }
    }
}
