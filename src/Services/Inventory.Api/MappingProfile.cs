using AutoMapper;
using Inventory.Api.Entities;
using Shared.DTOs.Inventory;

namespace Inventory.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<InventoryEntry, InventoryEntryDTO>().ReverseMap();
            CreateMap<PurchaseProductDto, InventoryEntryDTO>();
        }
    }
}
