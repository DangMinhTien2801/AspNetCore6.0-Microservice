using Contracts.Domain;
using Contracts.Domain.Intefaces;
using Infrastructure.Extensions;
using MongoDB.Driver;
using Shared.Configurations;
using System.Linq.Expressions;

namespace Infrastructure.Common.Repositories
{
    public class MongoDbRepository<T> : IMongoDbRepositoryBase<T> where T : MongoEntity
    {
        private IMongoDatabase Database { get; }
        public MongoDbRepository(IMongoClient client, MongoDbSettings settings)
        {
            Database = client.GetDatabase(settings.DatabaseName);
        }
        public Task CreateAsync(T obj) => Collection.InsertOneAsync(obj);

        public Task DeleteAsync(string id) => Collection.DeleteOneAsync(x => x.Id.Equals(id));

        public IMongoCollection<T> FindAll(ReadPreference? readPreference = null)
        {
            return Database.WithReadPreference(readPreference ?? ReadPreference.Primary).GetCollection<T>(GetCollectionName());
        }

        protected virtual IMongoCollection<T> Collection => Database.GetCollection<T>(GetCollectionName());

        public Task UpdateAsync(T obj)
        {
            Expression<Func<T, string>> func = f => f.Id;
            var value = (string)obj.GetType()
                .GetProperty(func.Body.ToString()
                .Split(".")[1])?.GetValue(obj, null);

            var filter = Builders<T>.Filter.Eq(func, value);

            return Collection.ReplaceOneAsync(filter, obj);
        }

        private static string GetCollectionName()
        {
            return (typeof(T).GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault() as BsonCollectionAttribute)?.CollectionName ?? string.Empty;
        }
    }
}
