namespace Saga.Orchestrator.OrderManager
{
    public enum EOrderTransactionState
    {
        NotStarted,
        BasketGot,
        BasketGetFailed,
        OrderCreated,
        OrderCreatedFailed,
        OrderGot,
        OrderGetFailed,
        InventoryUpdated,
        InventoryUpdateFailed,
        InventoryRollback
    }
}
