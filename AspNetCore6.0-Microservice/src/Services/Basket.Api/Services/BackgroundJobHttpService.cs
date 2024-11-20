using Shared.Configurations;

namespace Basket.Api.Services
{
    public class BackgroundJobHttpService
    {
        public HttpClient Client { get; set; }
        public BackgroundJobHttpService(
            HttpClient client,
            BackgroundJobSettings backgroundJobSettings)
        {
            client.BaseAddress = new Uri(backgroundJobSettings.HangfireUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            Client = client;
        }
    }
}
