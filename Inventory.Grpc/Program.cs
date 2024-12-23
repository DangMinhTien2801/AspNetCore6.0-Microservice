using Common.Logging;
using Inventory.Grpc.Extensions;
using Inventory.Grpc.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Serilogger.Configure);

Log.Information($"Start {builder.Environment.ApplicationName} up");

try
{
    // Additional configuration is required to successfully run gRPC on macOS.
    // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

    // Add services to the container.
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.ConfigureMongoDBClient();
    builder.Services.AddInfrastructureServices();
    builder.Services.AddGrpc();

    //builder.WebHost.ConfigureKestrel(options =>
    //{
    //    options.ListenLocalhost(5007, o => o.Protocols = HttpProtocols.Http2);
    //});

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.MapGrpcService<InventoryService>();
    app.MapGet("/", 
        () => 
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

    app.Run();
}
catch(Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;

    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information($"Shut down {builder.Environment.ApplicationName} complete");
    Log.CloseAndFlush();
}
