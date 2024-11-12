using Contracts.Domain.Intefaces;
using MongoDB.Bson.Serialization.Attributes;

namespace Inventory.Api.Entities.Abstraction
{
    public abstract class MongoEntity
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonElement("_id")]
        public virtual string Id { get; protected init; }

        [BsonElement("creationDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedDate { get ; set ; } = DateTime.UtcNow;
        [BsonElement("lastModifiedDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? LasrModifiedDate { get; set; } = DateTime.UtcNow;
    }
}
