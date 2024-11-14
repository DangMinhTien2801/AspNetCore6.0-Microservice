using AutoMapper;
using EventBus.Messages.IntegrationEvents.Events;
using MediatR;
using Odering.Application.Common.Mappings;
using Odering.Application.Features.V1.Orders.Common;
using Ordering.Domain.Entities;
using Shared.SeedWork;

namespace Odering.Application.Features.V1.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand
        : CreateOrUpdateCommand,
        IRequest<ApiResult<string>>,
        IMapForm<Order>,
        IMapForm<BasketCheckoutEvent>
    {
        public string UserName { get; set; } = null!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateOrderCommand, Order>().ReverseMap();
            profile.CreateMap<CreateOrderCommand, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
