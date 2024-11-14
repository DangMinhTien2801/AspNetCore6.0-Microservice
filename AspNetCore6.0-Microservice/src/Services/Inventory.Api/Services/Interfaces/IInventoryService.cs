using Contracts.Domain.Intefaces;
using Infrastructure.Common.Models;
using Inventory.Api.Entities;
using Shared.DTOs.Inventory;

namespace Inventory.Api.Services.Interfaces
{
    public interface IInventoryService : IMongoDbRepositoryBase<InventoryEntry>
    {
        Task<IEnumerable<InventoryEntryDTO>> GetAllByItemNoAsync(string itemNo);
        Task<PagedList<InventoryEntryDTO>> GetAllByItemNoPagingAsync(GetInventoryPagingQuery query);
        Task<InventoryEntryDTO> GetByIdAsync(string id);
        Task<InventoryEntryDTO> PurchaseItemAsync(string itemNo, PurchaseProductDto model);
    }
}
