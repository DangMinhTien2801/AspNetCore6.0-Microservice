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

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Serilogger.Configure);

Log.Information("Start Customer API up");
try
{
    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
    builder.Services.AddDbContext<CustomerContext>(options =>
    {
        options.UseNpgsql(connectionString ??
            "Server=localhost;Port=5432;Database=CustomerDb;User Id=admin;Password=TienHaui@1234");
    });
    // đăng ký DI
    builder.Services.AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>))
                .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
                .AddScoped<ICustomerRepository, CustomerRepository>()
                .AddScoped<ICustomerService, CustomerService>();
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
