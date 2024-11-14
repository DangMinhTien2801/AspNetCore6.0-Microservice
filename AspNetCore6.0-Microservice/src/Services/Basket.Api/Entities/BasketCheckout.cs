namespace Basket.Api.Entities
{
    public class BasketCheckout
    {
        public string UserName { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string ShippingAddress { get; set; } = null!;
        public string InvoiceAddress { get; set; } = null!;
    }
}
