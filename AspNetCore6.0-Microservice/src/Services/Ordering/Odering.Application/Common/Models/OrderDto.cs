using AutoMapper;
using Odering.Application.Common.Mappings;
using Ordering.Domain.Entities;
using Shared.Enum.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odering.Application.Common.Models
{
    public class OrderDto : IMapForm<Order>
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public decimal TotalPrice { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;

        public string ShippingAddress { get; set; } = null!;
        public string InvoiceAddress { get; set; } = null!;

        public EOrderStatus Status { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Order, OrderDto>().ReverseMap();
        }
    }
}
