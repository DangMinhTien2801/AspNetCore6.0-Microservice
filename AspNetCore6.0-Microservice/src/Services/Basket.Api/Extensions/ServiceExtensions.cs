using Basket.Api.Repositories;
using Basket.Api.Repositories.Interfaces;
using Basket.Api.Services;
using Basket.Api.Services.Interfaces;
using Contracts.Common.Interfaces;
using EventBus.Messages.IntegrationEvents.Interfaces;
using Infrastructure.Common;
using Infrastructure.Configurations;
using Infrastructure.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Configurations;

namespace Basket.Api.Extensions
{
    public static class ServiceExtensions
    {
        internal static IServiceCollection AddConfigurationSettings(
            this IServiceCollection services, IConfiguration configuration)
        {
            var eventBusSettings = configuration.GetSection(nameof(EventBusSettings))
                .Get<EventBusSettings>();
            services.AddSingleton(eventBusSettings);

            var cacheSettings = configuration.GetSection(nameof(CacheSettings))
                .Get<CacheSettings>();
            services.AddSingleton(cacheSettings);

            var backgroundJobSettings = configuration.GetSection(nameof(BackgroundJobSettings))
                .Get<BackgroundJobSettings>();
            services.AddSingleton(backgroundJobSettings);

            return services;
        }
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IBasketRepository, BasketRepository>()
                .AddTransient<ISerializeService, SerializeService>()
                .AddTransient<IEmailTemplateService, BasketEmailTemplateService>();

            return services;
        }
        public static void ConfigureHttpClientService(this IServiceCollection services)
        {
            services.AddHttpClient<BackgroundJobHttpService>();
        }
        public static void ConfigureRedis(this IServiceCollection services,
            IConfiguration configuration)
        {
            var settings = services
                .GetOptions<CacheSettings>("CacheSettings");
            if(string.IsNullOrEmpty(settings.ConnectionString))
            {
                throw new ArgumentNullException("Redis connectionString is not configured");
            }
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = settings.ConnectionString;
            });
        }
        public static void ConfigureMassTransit(this IServiceCollection services)
        {
            var settings = services.GetOptions<EventBusSettings>("EventBusSettings");
            if(settings == null || string.IsNullOrEmpty(settings.HostAddress))
            {
                throw new ArgumentNullException("EventBusSettins is not configured");
            }
            var mqConnection = new Uri(settings.HostAddress);
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((cxt, cfg) =>
                {
                    cfg.Host(mqConnection);
                });
                // Publish submit order message
                config.AddRequestClient<IBasketCheckoutEvent>();
            });
        }
    }
}
