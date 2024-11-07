using EventBus.Messages.IntegrationEvents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.IntegrationEvents.Events
{
    public record BasketCheckoutEvent() : IntegrationBaseEvent, IBasketCheckoutEvent
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
