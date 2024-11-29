using Shared.Configurations;

namespace Basket.Api.Services
{
    public class BackgroundJobHttpService
    {
        public HttpClient Client { get; }
        public string ScheduledJobUrl { get; }
        public BackgroundJobHttpService(
            HttpClient client,
            BackgroundJobSettings backgroundJobSettings)
        {
            client.BaseAddress = new Uri(backgroundJobSettings.HangfireUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            Client = client;

            ScheduledJobUrl = backgroundJobSettings.ScheduledJobUrl;
        }
    }
}
