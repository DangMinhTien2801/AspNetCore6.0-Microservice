﻿namespace Basket.Api.Entities
{
    public class Cart
    {
        public string UserName { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public List<CartItem> Items { get; set; } = new();
        public Cart()
        {
        }
        public Cart(string userName)
        {
            UserName = userName;
        }
        public decimal TotalPrice
        {
            get
            {
                return Items.Sum(i => i.ItemPrice * i.Quantity);
            }
        }
        public DateTimeOffset LastModifiedDate { get; set; } = DateTimeOffset.UtcNow;
        public string? JobId { get; set; }
    }
}
