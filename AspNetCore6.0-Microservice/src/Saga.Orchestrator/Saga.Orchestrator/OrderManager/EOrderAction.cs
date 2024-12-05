namespace Saga.Orchestrator.OrderManager
{
    public enum EOrderAction
    {
        GetBasket,
        CreateOrder,
        GetOrder,
        UpdateInventory,
        RollbackInventory,
        DeleteInventory,
        DeleteBasket,
        DeleteOrder
    }
}
