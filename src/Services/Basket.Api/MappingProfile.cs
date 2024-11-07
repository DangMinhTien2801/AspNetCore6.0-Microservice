using AutoMapper;
using Basket.Api.Entities;
using EventBus.Messages.IntegrationEvents.Events;

namespace Basket.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>();
        }
    }
}
