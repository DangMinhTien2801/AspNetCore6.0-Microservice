using Infrastructure.Extensions;
using Inventory.Api.Services;
using Inventory.Api.Services.Interfaces;
using MongoDB.Driver;
using Shared.Configurations;
using System.Runtime.CompilerServices;

namespace Inventory.Api.Extensions
{
    public static class ServiceExtensions
    {
        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
            IConfiguration configuration)
        {
            var databaseSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            services.AddSingleton(databaseSettings);

            return services;
        }
        private static string GetMongoConnectionString(this IServiceCollection services)
        {
            var settings = services.GetOptions<MongoDbSettings>(nameof(MongoDbSettings));
            if(settings == null || string.IsNullOrEmpty(settings.ConnectionString))
            {
                throw new ArgumentNullException("DatabaseSettings is not configured");
            }
            var databaseName = settings.DatabaseName;
            var mongoDbConnectionString = settings.ConnectionString + "/" + databaseName + "?authSource=admin";

            return mongoDbConnectionString;
        }
        public static void ConfigureMongoDBClient(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(
                new MongoClient(GetMongoConnectionString(services)))
                .AddScoped(x => x.GetService<IMongoClient>()?.StartSession());  
        }
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { cfg.AddProfile(new MappingProfile()); });

            services.AddScoped<IInventoryService, InventoryService>();
        }
    }
}
