using Inventory.Api.Entities;
using Inventory.Api.Extensions;
using MongoDB.Driver;

namespace Inventory.Api.Persistence
{
    public class InventoryDbSeed
    {
        public async Task SeedDataAsync(IMongoClient mongoClient, DatabaseSettings settings)
        {
            var databaseName = settings.DatabaseName;
            var database = mongoClient.GetDatabase(databaseName);
            var inventoryCollection = database.GetCollection<InventoryEntry>("InventoryEntries");
            if(await inventoryCollection.EstimatedDocumentCountAsync() == 0)
            {
                await inventoryCollection.InsertManyAsync(GetPreconfiguredInventoryEntries());
            }
        }
        private IEnumerable<InventoryEntry> GetPreconfiguredInventoryEntries()
        {
            return new List<InventoryEntry>
            {
                new()
                {
                    Quantity = 10,
                    DocumentNo = Guid.NewGuid().ToString(),
                    ItemNo = "Lotus",
                    ExternalDocumentNo = Guid.NewGuid().ToString(),
                    DocumentType = Shared.Enum.Inventory.EDocumentType.Purchase
                },
                new()
                {
                    Quantity = 10,
                    DocumentNo = Guid.NewGuid().ToString(),
                    ItemNo = "Cadilac",
                    ExternalDocumentNo = Guid.NewGuid().ToString(),
                    DocumentType = Shared.Enum.Inventory.EDocumentType.Purchase
                }
            };
        }
    }
}
