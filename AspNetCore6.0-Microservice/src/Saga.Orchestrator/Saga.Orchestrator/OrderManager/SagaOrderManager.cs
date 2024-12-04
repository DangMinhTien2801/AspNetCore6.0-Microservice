using AutoMapper;
using Contracts.Sagas.OrderManager;
using Saga.Orchestrator.HttpRepository.Interfaces;
using Shared.DTOs.Basket;
using Shared.DTOs.Inventory;
using Shared.DTOs.Order;

namespace Saga.Orchestrator.OrderManager
{
    public class SagaOrderManager : ISagaOrderManager<BasketCheckoutDto, OrderResponse>
    {
        private readonly IOrderHttpRepository _orderHttpRepository;
        private readonly IBasketHttpRepository _basketHttpRepository;
        private readonly IInventoryHttpRepository _inventoryHttpRepository;
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;

        public SagaOrderManager(IOrderHttpRepository orderHttpRepository,
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
        public OrderResponse CreateOrder(BasketCheckoutDto input)
        {
            var orderStateMachne =
                new Stateless.StateMachine<EOrderTransactionState, EOrderAction>(EOrderTransactionState.NotStarted);
            string? orderId = null;
            CartDto cart = null;
            OrderDto addedOrder = null;
            string? inventoryDocumentNo;

            orderStateMachne.Configure(EOrderTransactionState.NotStarted)
                .PermitDynamic(EOrderAction.GetBasket, () =>
                {
                    cart = _basketHttpRepository.GetBasket(input.UserName).Result;
                    return cart != null ? EOrderTransactionState.BasketGot : EOrderTransactionState.BasketGetFailed;
                });

            orderStateMachne.Configure(EOrderTransactionState.BasketGot)
                .PermitDynamic(EOrderAction.CreateOrder, () =>
                {
                    var order = _mapper.Map<CreateOrderDto>(input);
                    orderId = _orderHttpRepository.CreateOrder(order).Result;
                    return orderId == null ? EOrderTransactionState.OrderCreatedFailed : EOrderTransactionState.OrderCreated;
                })
                .OnEntry(() => orderStateMachne.Fire(EOrderAction.CreateOrder));

            orderStateMachne.Configure(EOrderTransactionState.OrderCreated)
                .PermitDynamic(EOrderAction.GetOrder, () =>
                {
                    addedOrder = _orderHttpRepository.GetOrder(orderId).Result;
                    return addedOrder != null ? EOrderTransactionState.OrderGot : EOrderTransactionState.OrderGetFailed;
                })
                .OnEntry(() => orderStateMachne.Fire(EOrderAction.GetOrder));

            orderStateMachne.Configure(EOrderTransactionState.OrderGot)
                .PermitDynamic(EOrderAction.UpdateInventory, () =>
                {
                    var salesOrder = new SalesOrderDto
                    {
                        OrderNo = addedOrder.DocumentNo,
                        SaleItems = _mapper.Map<List<SaleItemDto>>(cart.Items)
                    };

                    return EOrderTransactionState.OrderCreated;
                });

            return null;
        }

        public OrderResponse RollbackOrder(BasketCheckoutDto input)
        {
            throw new NotImplementedException();
        }
    }
}
