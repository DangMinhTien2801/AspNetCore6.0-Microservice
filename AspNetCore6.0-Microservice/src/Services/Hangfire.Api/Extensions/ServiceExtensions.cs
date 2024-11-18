using Shared.Configurations;

namespace Hangfire.Api.Extensions
{
    public static class ServiceExtensions
    {
        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
            IConfiguration configuration)
        {
            var hangfireSettings = configuration.GetSection(nameof(HangfireSettings)).Get<HangfireSettings>();
            services.AddSingleton(hangfireSettings);

            return services;
        }
    }
}
