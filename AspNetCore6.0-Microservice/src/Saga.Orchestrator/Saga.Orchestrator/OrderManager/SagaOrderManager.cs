﻿using AutoMapper;
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
            var orderStateMachine =
                new Stateless.StateMachine<EOrderTransactionState, EOrderAction>(EOrderTransactionState.NotStarted);
            string? orderId = null;
            CartDto cart = null;
            OrderDto addedOrder = null;
            string? inventoryDocumentNo = null;

            orderStateMachine.Configure(EOrderTransactionState.NotStarted)
                .PermitDynamic(EOrderAction.GetBasket, () =>
                {
                    cart = _basketHttpRepository.GetBasket(input.UserName).Result;
                    return cart != null ? EOrderTransactionState.BasketGot : EOrderTransactionState.BasketGetFailed;
                });

            orderStateMachine.Configure(EOrderTransactionState.BasketGot)
                .PermitDynamic(EOrderAction.CreateOrder, () =>
                {
                    //input.TotalPrice = cart?.TotalPrice ?? 0; // use this is fine
                    var order = _mapper.Map<CreateOrderDto>(input);
                    order.TotalPrice = cart?.TotalPrice ?? 0;
                    orderId = _orderHttpRepository.CreateOrder(order).Result;
                    return orderId == null ? EOrderTransactionState.OrderCreatedFailed : EOrderTransactionState.OrderCreated;
                })
                .OnEntry(() => orderStateMachine.Fire(EOrderAction.CreateOrder));

            orderStateMachine.Configure(EOrderTransactionState.OrderCreated)
                .PermitDynamic(EOrderAction.GetOrder, () =>
                {
                    addedOrder = _orderHttpRepository.GetOrder(orderId).Result;
                    return addedOrder != null ? EOrderTransactionState.OrderGot : EOrderTransactionState.OrderGetFailed;
                })
                .OnEntry(() => orderStateMachine.Fire(EOrderAction.GetOrder));

            orderStateMachine.Configure(EOrderTransactionState.OrderGot)
                .PermitDynamic(EOrderAction.UpdateInventory, () =>
                {
                    var salesOrder = new SalesOrderDto
                    {
                        OrderNo = addedOrder.DocumentNo,
                        SaleItems = _mapper.Map<List<SaleItemDto>>(cart.Items)
                    };

                    inventoryDocumentNo = _inventoryHttpRepository.CreateOrderSale(addedOrder.DocumentNo, salesOrder).Result;
                    return inventoryDocumentNo != null 
                    ? EOrderTransactionState.InventoryUpdated
                    : EOrderTransactionState.InventoryUpdateFailed;
                }).OnEntry(() => orderStateMachine.Fire(EOrderAction.UpdateInventory));

            orderStateMachine.Configure(EOrderTransactionState.InventoryUpdated)
                .PermitDynamic(EOrderAction.DeleteBasket, () =>
                {
                    var result = _basketHttpRepository.DeleteBasket(input.UserName).Result;
                    return result ? EOrderTransactionState.BasketDeleted : EOrderTransactionState.InventoryUpdateFailed;
                }).OnEntry(() => orderStateMachine.Fire(EOrderAction.DeleteBasket));

            orderStateMachine.Configure(EOrderTransactionState.InventoryUpdateFailed)
                .PermitDynamic(EOrderAction.DeleteInventory, () =>
                {
                    RollbackOrder(input.UserName, inventoryDocumentNo, orderId);
                    return EOrderTransactionState.InventoryRollback;
                })
                .OnEntry(() => orderStateMachine.Fire(EOrderAction.DeleteInventory));

            orderStateMachine.Fire(EOrderAction.GetBasket);

            return new OrderResponse(orderStateMachine.State == EOrderTransactionState.BasketDeleted);
        }

        public OrderResponse RollbackOrder(string userName, string documentNo, string orderId)
        {
            var orderStateMachine = new Stateless.StateMachine<EOrderTransactionState, EOrderAction>(EOrderTransactionState.RollbackInventory);

            orderStateMachine.Configure(EOrderTransactionState.RollbackInventory)
                .PermitDynamic(EOrderAction.DeleteInventory, () =>
                {
                    _inventoryHttpRepository.DeleteOrderByDocumentNo(documentNo);
                    return EOrderTransactionState.InventoryRollback;
                });

            orderStateMachine.Configure(EOrderTransactionState.InventoryRollback)
                .PermitDynamic(EOrderAction.DeleteOrder,() =>
                {
                    var result = _orderHttpRepository.DeleteOrder(orderId).Result;
                    return result
                        ? EOrderTransactionState.OrderDeleted
                        : EOrderTransactionState.OrderDeletedFailed;
                }).OnEntry(() => orderStateMachine.Fire(EOrderAction.DeleteOrder));

            orderStateMachine.Fire(EOrderAction.DeleteInventory);

            return new OrderResponse(orderStateMachine.State == EOrderTransactionState.InventoryRollback);
        }
    }
}
