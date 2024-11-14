using Contracts.Domain;
using Infrastructure.Extensions;
using Inventory.Api.Extensions;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Enum.Inventory;

namespace Inventory.Api.Entities
{
    [BsonCollection("InventoryEntries")]
    public class InventoryEntry : MongoEntity
    {
        public InventoryEntry()
        {
            DocumentType = EDocumentType.Purchase;
            DocumentNo = Guid.NewGuid().ToString();
            ExternalDocumentNo = Guid.NewGuid().ToString(); 
        }
        public InventoryEntry(string id) => (Id) = id;

        [BsonElement("documentType")]
        public EDocumentType DocumentType {  get; set; }

        [BsonElement("documentNo")]
        public string DocumentNo { get; set; }

        [BsonElement("itemNo")]
        public string ItemNo { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }

        [BsonElement("externalDocumentNo")]
        public string ExternalDocumentNo { get; set; }
    }
}
