﻿using Shared.DTOs.Basket;
using Shared.DTOs.Inventory;

namespace Saga.Orchestrator.HttpRepository.Interfaces
{
    public interface IInventoryHttpRepository
    {
        Task<string> CreateSalesOrder(SalesProductDto model);
        Task<bool> DeleteOrderByDocumentNo(string documentNo);
        Task<string> CreateOrderSale(string orderNo, SalesOrderDto model);
    }
}
