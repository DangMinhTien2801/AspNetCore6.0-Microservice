﻿using AutoMapper;
using Infrastructure.Common.Models;
using Infrastructure.Common.Repositories;
using Infrastructure.Extensions;
using Inventory.Api.Entities;
using Inventory.Api.Extensions;
using Inventory.Api.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Configurations;
using Shared.DTOs.Inventory;

namespace Inventory.Api.Services
{
    public class InventoryService : MongoDbRepository<InventoryEntry>, IInventoryService
    {
        private readonly IMapper _mapper;
        public InventoryService(IMongoClient client,
            MongoDbSettings settings,
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

        public async Task<PagedList<InventoryEntryDTO>> GetAllByItemNoPagingAsync(GetInventoryPagingQuery query)
        {
            var filterSearchTerm = Builders<InventoryEntry>.Filter.Empty;
            var filterItemNo = Builders<InventoryEntry>.Filter.Eq(x => x.ItemNo, query.ItemNo());
            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                filterSearchTerm = Builders<InventoryEntry>.Filter.Eq(x => x.DocumentNo, query.SearchTerm);
            }
            var andFilter = filterItemNo & filterSearchTerm;
            var pagedList = await Collection.PaginatedListAsync(andFilter, query.PageIndex, query.PageSize);

            var items = _mapper.Map<IEnumerable<InventoryEntryDTO>>(pagedList);
            var result = new PagedList<InventoryEntryDTO>(items, pagedList.GetMetaData().TotalItems, query.PageIndex, query.PageSize);

            return result;
        }

        public async Task<InventoryEntryDTO> GetByIdAsync(string id)
        {
            FilterDefinition<InventoryEntry> filter = Builders<InventoryEntry>.Filter.Eq(x => x.Id, id);
            var entity = await FindAll().Find(filter).FirstOrDefaultAsync();
            var result = _mapper.Map<InventoryEntryDTO>(entity);

            return result;
        }

        public async Task<InventoryEntryDTO> PurchaseItemAsync(string itemNo, PurchaseProductDto model)
        {
            var entity = new InventoryEntry(ObjectId.GenerateNewId().ToString())
            {
                ItemNo = model.ItemNo,
                Quantity = model.Quantity,
                DocumentType = model.DocumentType,
            };
            await CreateAsync(entity);
            var result = _mapper.Map<InventoryEntryDTO>(entity);

            return result;
        }
    }
}