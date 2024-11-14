using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureLogs();
builder.Host.UseSerilog();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

#region helper
void ConfigureLogs()
{
    // Get the enviroment which the appliications is running on
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIROMENT");
    // Get the configuration
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();
    // Create Logger
    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(ConfigureELS(configuration, env))
        .CreateLogger();
        
}
ElasticsearchSinkOptions ConfigureELS(IConfigurationRoot configuration, string env)
{
    return new ElasticsearchSinkOptions(new Uri(configuration["ELKConfiguration:Uri"]))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"{DateTime.UtcNow:yyyy-MM}"
    };
}
#endregion
