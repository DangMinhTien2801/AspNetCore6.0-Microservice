namespace Inventory.Api.Extensions
{
    public class DatabaseSettings : Shared.Configurations.DatabaseSettings
    {
        public string DatabaseName { get; set; } = null!;
    }
}
