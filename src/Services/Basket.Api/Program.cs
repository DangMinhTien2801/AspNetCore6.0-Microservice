using Serilog;
using Common.Logging;
using Basket.Api.Extensions;
using Basket.Api;

Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

Log.Information($"Start {builder.Environment.ApplicationName} up");
try
{
    builder.Host.UseSerilog(Serilogger.Configure);
    // Add services to the container.
    builder.Host.AddAppConfigurations();
    builder.Services.AddAutoMapper(config =>
    {
        config.AddProfile(new MappingProfile());
    });

    builder.Services.ConfigureServices();
    builder.Services.AddControllers();
    builder.Services.ConfigureRedis(builder.Configuration);
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.Configure<RouteOptions>(options =>
    {
        options.LowercaseUrls = true;
    });
    // Configure Mass Transit
    builder.Services.ConfigureMassTransit();
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
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down Basket API complete");
    Log.CloseAndFlush();
}
