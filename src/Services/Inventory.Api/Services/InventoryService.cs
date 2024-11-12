using AutoMapper;
using Inventory.Api.Entities;
using Inventory.Api.Extensions;
using Inventory.Api.Repositories.Abstraction;
using Inventory.Api.Services.Interfaces;
using MongoDB.Driver;
using Shared.DTOs.Inventory;

namespace Inventory.Api.Services
{
    public class InventoryService : MongoDbRepository<InventoryEntry>, IInventoryServices
    {
        private readonly IMapper _mapper;
        public InventoryService(IMongoClient client,
            DatabaseSettings settings,
            IMapper mapper) : base(client, settings)
        {
            _mapper = mapper;   
        }
        public async Task<IEnumerable<InventoryEntryDTO>> GetAllByItemNoAsync(string itemNo)
        {
            var entities = await FindAll().Find(x => x.ItemNo.Equals(itemNo)).ToListAsync();
            var result = _mapper.Map<IEnumerable<InventoryEntryDTO>>(entities);
            return result;
        }

        public async Task<IEnumerable<InventoryEntryDTO>> GetAllByItemNoPagingAsync(GetInventoryPagingQuery query)
        {
            var filterSearchTerm = Builders<InventoryEntry>.Filter.Empty;
            var filterItemNo = Builders<InventoryEntry>.Filter.Eq(x => x.ItemNo, query.ItemNo());
            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                filterSearchTerm = Builders<InventoryEntry>.Filter.Eq(x => x.DocumentNo, query.SearchTerm);
            }
            var andFilter = filterItemNo & filterSearchTerm;
            var pagedList = await Collection.Find(andFilter)
                .Skip((query.PageIndex - 1) * query.PageSize)
                .Limit(query.PageSize)
                .ToListAsync();

            var result = _mapper.Map<IEnumerable<InventoryEntryDTO>>(pagedList);
            return result;
        }

        public Task<InventoryEntryDTO> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<InventoryEntryDTO> PurchaseItemAsync(string itemNo, PurchaseProductDto model)
        {
            throw new NotImplementedException();
        }
    }
}
