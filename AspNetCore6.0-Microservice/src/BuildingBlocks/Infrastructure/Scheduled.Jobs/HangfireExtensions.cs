using Hangfire;
using Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire.Console;
using Newtonsoft.Json;
using Hangfire.Console.Extensions;
using Hangfire.PostgreSql;

namespace Infrastructure.Scheduled.Jobs
{
    public static class HangfireExtensions
    {
        public static IServiceCollection AddHangfireService(this IServiceCollection services)
        {
            var settings = services.GetOptions<HangfireSettings>("HangFireSettings");
            if(settings == null || settings.Storage == null ||
                string.IsNullOrEmpty(settings.Storage.ConnectionString))
            {
                throw new Exception("Hangfire settings is not configured properly !");
            }
            services.ConfigureHangfireService(settings);
            services.AddHangfireServer(serverOptions =>
            {
                serverOptions.ServerName = settings.ServerName;
            });

            return services;
        }

        private static IServiceCollection ConfigureHangfireService(this IServiceCollection services, HangfireSettings settings)
        {
            if (string.IsNullOrEmpty(settings.Storage.DbProvider))
                throw new Exception("Hangfire DBProvider is not configred.");

            switch (settings.Storage.DbProvider.ToLower())
            {
                case "mongodb":
                    var mongoUrlBuilder = new MongoUrlBuilder(settings.Storage.ConnectionString);

                    var mongoClientSettings = MongoClientSettings.FromUrl(
                        new MongoUrl(settings.Storage.ConnectionString));
                    mongoClientSettings.SslSettings = new SslSettings
                    {
                        EnabledSslProtocols = SslProtocols.Ssl2
                    };
                    var mongoCLient = new MongoClient(mongoClientSettings);

                    var mongoStorageOptions = new MongoStorageOptions
                    {
                        MigrationOptions = new MongoMigrationOptions
                        {
                            MigrationStrategy = new MigrateMongoMigrationStrategy(),
                            BackupStrategy = new CollectionMongoBackupStrategy()
                        },
                        CheckConnection = true,
                        Prefix = "SchedulerQueue",
                        CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
                    };
                    services.AddHangfire((provider, config) =>
                    {
                        config.UseSimpleAssemblyNameTypeSerializer()
                              .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                              .UseRecommendedSerializerSettings()
                              .UseConsole()
                              .UseMongoStorage(mongoCLient, mongoUrlBuilder.DatabaseName, mongoStorageOptions);

                        var jsonSettings = new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        };
                        config.UseSerializerSettings(jsonSettings);
                    });
                    services.AddHangfireConsoleExtensions();

                    break;
                case "postgresql":
                    services.AddHangfire(x =>
                        x.UsePostgreSqlStorage(settings.Storage.ConnectionString));
                    break;
                case "smsql":
                    break;
                default:
                    throw new Exception($"Hangfire Storage Provider {settings.Storage.DbProvider} is not supported");
            }

            return services;
        }
    }
}
