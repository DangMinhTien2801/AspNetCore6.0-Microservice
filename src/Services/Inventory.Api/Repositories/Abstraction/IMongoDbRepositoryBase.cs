using Inventory.Api.Entities.Abstraction;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MongoDB.Driver;

namespace Inventory.Api.Repositories.Abstraction
{
    public interface IMongoDbRepositoryBase<T> where T : MongoEntity
    {
        IMongoCollection<T> FindAll(ReadPreference? readPreference = null);
        Task CreateAsync(T obj);
        Task UpdateAsync(T obj);
        Task DeleteAsync(string id);
    }
}
