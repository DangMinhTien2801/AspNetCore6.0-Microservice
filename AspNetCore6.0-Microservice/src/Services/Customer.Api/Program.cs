using Serilog;
using Common.Logging;
using Customer.Api.Persistence;
using Microsoft.EntityFrameworkCore;
using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Customer.Api.Reponsitories.Interfaces;
using Customer.Api.Reponsitories;
using Customer.Api.Services.Interfaces;
using Customer.Api.Services;
using Customer.Api;
using Shared.DTOs.Customer;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Customer.Api.Controllers;
using Infrastructure.Common.Repositories;
using Customer.Api.Extensions;
using Infrastructure.Scheduled.Jobs;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Serilogger.Configure);

Log.Information("Start Customer API up");
try
{
    // Add services to the container.
    builder.Services.AddConfigurationSettings(builder.Configuration);
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.ConfigureCustomerContext();
    // đăng ký DI
    builder.Services.AddInfrastructureServices();
    // đăng ký hangfire
    builder.Services.AddHangfireService();
    // đăng ký mapper
    builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
    var app = builder.Build();
    app.MapCustomersAPI();  
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseHangfireDashboard(app.Configuration);

    app.MapControllers();

    app.SeedCustomerData()
        .Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;

    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down Customer API complete");
    Log.CloseAndFlush();
}
