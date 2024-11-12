using Inventory.Api.Entities;
using Inventory.Api.Repositories.Abstraction;
using Shared.DTOs.Inventory;

namespace Inventory.Api.Services.Interfaces
{
    public interface IInventoryServices : IMongoDbRepositoryBase<InventoryEntry>
    {
        Task<IEnumerable<InventoryEntryDTO>> GetAllByItemNoAsync(string itemNo);
        Task<IEnumerable<InventoryEntryDTO>> GetAllByItemNoPagingAsync(GetInventoryPagingQuery query);
        Task<InventoryEntryDTO> GetByIdAsync(string id);
        Task<InventoryEntryDTO> PurchaseItemAsync(string itemNo, PurchaseProductDto model);
    }
}
