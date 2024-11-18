using Contracts.Common.Interfaces;
using Customer.Api.Persistence;
using Customer.Api.Reponsitories;
using Customer.Api.Reponsitories.Interfaces;
using Customer.Api.Services;
using Customer.Api.Services.Interfaces;
using Infrastructure.Common.Repositories;
using Infrastructure.Common;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Shared.Configurations;

namespace Customer.Api.Extensions
{
    public static class ServiceExtensions
    {
        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
            IConfiguration configuration)
        {
            var databaseSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
            services.AddSingleton(databaseSettings);

            var hangfireSettings = configuration.GetSection(nameof(HangfireSettings)).Get<HangfireSettings>();
            services.AddSingleton(hangfireSettings);

            return services;
        }

        public static void ConfigureCustomerContext(this IServiceCollection services)
        {
            var databaseSettings = services.GetOptions<DatabaseSettings>(nameof(DatabaseSettings));
            if (databaseSettings == null || string.IsNullOrEmpty(databaseSettings.ConnectionString))
                throw new ArgumentNullException("Connection string is not configred");

            services.AddDbContext<CustomerContext>(options =>
            {
               options.UseNpgsql(databaseSettings.ConnectionString);
            });
        }

        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>()
                    .AddScoped<ICustomerService, CustomerService>()
                    .AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>))
                    .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        }
    }
}
