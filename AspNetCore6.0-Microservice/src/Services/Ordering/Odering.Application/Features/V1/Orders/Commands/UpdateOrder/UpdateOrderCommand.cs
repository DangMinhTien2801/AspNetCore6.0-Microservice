using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Odering.Application.Common.Mappings;
using Odering.Application.Common.Models;
using Odering.Application.Features.V1.Orders.Common;
using Ordering.Domain.Entities;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odering.Application.Features.V1.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand
        : CreateOrUpdateCommand,
        IRequest<ApiResult<OrderDto>>,
        IMapForm<Order>
    {
        public string Id { get; private set; } = null!;
        public void SetId(string id)
        {
            Id = id;
        }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateOrderCommand, Order>()
                .ForMember(dest => dest.Status, opts => opts.Ignore())
                .IgnoreAllNonExisting()
                .ReverseMap();
        }
    }
}
