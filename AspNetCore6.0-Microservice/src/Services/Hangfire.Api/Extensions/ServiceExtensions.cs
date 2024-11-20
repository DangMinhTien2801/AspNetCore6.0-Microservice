using Contracts.ScheduledJobs;
using Contracts.Services;
using Hangfire.Api.Services;
using Hangfire.Api.Services.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.Scheduled.Jobs;
using Infrastructure.Services;
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

            var emailSettings = configuration.GetSection(nameof(SmtpEmailSetting))
                .Get<SmtpEmailSetting>();
            services.AddSingleton(emailSettings);

            return services;
        }
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IScheduledJobService, HangfireService>();
            services.AddScoped(typeof(ISmtpEmailService), typeof(SmtpEmailService));
            services.AddTransient<IBackgroundJobService, BackgroundJobService>();

            return services;
        }
    }
}
