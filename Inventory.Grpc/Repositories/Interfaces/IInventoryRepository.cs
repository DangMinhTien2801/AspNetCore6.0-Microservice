using Contracts.Domain.Intefaces;
using Inventory.Grpc.Entities;
using MongoDB.Driver;

namespace Inventory.Grpc.Repositories.Interfaces
{
    public interface IInventoryRepository : IMongoDbRepositoryBase<InventoryEntry>
    {
        Task<int> GetStockQuantity(string itemNo); 
    }
}
