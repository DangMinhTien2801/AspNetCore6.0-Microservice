using Hangfire;
using Shared.Configurations;

namespace Customer.Api.Extensions
{
    public static class HostExtensions
    {
        internal static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app, IConfiguration configuration)
        {
            var configDashboard = configuration.GetSection("HangFireSettings:Dashboard").Get<DashboardOptions>();
            var hangFireSettings = configuration.GetSection("HangFireSettings").Get<HangfireSettings>();
            var hangFireRoute = hangFireSettings.Route;

            app.UseHangfireDashboard(hangFireRoute, new DashboardOptions
            {
                DashboardTitle = configDashboard.DashboardTitle,
                StatsPollingInterval = configDashboard.StatsPollingInterval,
                AppPath = configDashboard.AppPath,
                IgnoreAntiforgeryToken = true
            });

            return app;
        }
    }
}
