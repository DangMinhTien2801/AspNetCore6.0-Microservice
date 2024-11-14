using Infrastructure.Common.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public static class MongoCollectionExtensions
    {
        public static Task<PagedList<TDestination>> PaginatedListAsync<TDestination>(
            this IMongoCollection<TDestination> collection,
            FilterDefinition<TDestination> filter,
            int pageIndex,
            int pageSize) where TDestination : class
        => PagedList<TDestination>.ToPagedList(collection, filter, pageIndex, pageSize);
    }
}
