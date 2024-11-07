using AutoMapper;
using Odering.Application.Common.Mappings;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odering.Application.Features.V1.Orders.Common
{
    public class CreateOrUpdateCommand : IMapForm<Order>
    {
        public decimal TotalPrice { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string ShippingAddress { get; set; } = null!;

        public string InvoiceAddress { get; set; } = null!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateOrUpdateCommand, Order>()
                .ReverseMap();
        }
    }
}
