using AutoMapper;
using Basket.Api.Entities;
using EventBus.Messages.IntegrationEvents.Events;
using Shared.DTOs.Basket;

namespace Basket.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>();
            CreateMap<CartDto, Cart>().ReverseMap();
            CreateMap<CartItemDto, CartItem>().ReverseMap();
        }
    }
}
