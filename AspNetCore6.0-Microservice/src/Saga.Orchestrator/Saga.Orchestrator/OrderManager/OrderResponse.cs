namespace Saga.Orchestrator.OrderManager
{
    public class OrderResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public OrderResponse(bool success, string message)
        {
            Success = success;
            Message = message; 
        }

    }
}
