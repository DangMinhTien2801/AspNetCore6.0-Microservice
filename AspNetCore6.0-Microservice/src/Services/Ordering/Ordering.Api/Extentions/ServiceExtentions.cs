using Contracts.Configurations;
using EventBus.Messages.IntegrationEvents.Events;
using Infrastructure.Configurations;
using Infrastructure.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ordering.Api.Application.IntegrationEvents.EventsHandler;
using Shared.Configurations;

namespace Ordering.Api.Extentions
{
    public static class ServiceExtentions
    {
        internal static IServiceCollection AddConfigurationSettings(
            this IServiceCollection services, IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection(nameof(SmtpEmailSetting))
                .Get<SmtpEmailSetting>();
            services.AddSingleton(emailSettings);


            var eventBusSettings = configuration.GetSection(nameof(EventBusSettings))
                .Get<EventBusSettings>();
            services.AddSingleton(eventBusSettings);

            return services;
        }
        public static void ConfigureMassTransit(this IServiceCollection services)
        {
            var settings = services.GetOptions<EventBusSettings>("EventBusSettings");
            if(settings == null || string.IsNullOrEmpty(settings.HostAddress))
            {
                throw new ArgumentNullException("EventBusSettings is not configured");
            }
            var mqConnection = new Uri(settings.HostAddress);
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services.AddMassTransit(config =>
            {
                config.AddConsumersFromNamespaceContaining<BasketCheckoutEventHandler>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection);
                    //cfg.ReceiveEndpoint("basket-checkout-queue", c =>
                    //{
                    //    c.ConfigureConsumer<BasketCheckoutEventHandler>(ctx);
                    //});

                    cfg.ConfigureEndpoints(ctx);
                });
            });
        }
    }
}
