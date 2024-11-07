using Contracts.Common.Interfaces;
using Contracts.Messages;
using Contracts.Services;
using Infrastructure.Common;
using Infrastructure.Messages;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Odering.Application.Common.Interfaces;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<OrderContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString") ??
                    "Server=localhost,1434;Database=OrderDb;User Id=SA;Password=TienHaui@1234",
                    builder =>
                    {
                        builder.MigrationsAssembly(typeof(OrderContext).Assembly.FullName);
                    });
            });
            services.AddScoped<OrderContextSeed>();
            services.AddScoped<IOrderRepository, OrderRepository>()
                .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped(typeof(ISmtpEmailService), typeof(SmtpEmailService));
            services.AddScoped<ISerializeService, SerializeService>();
            services.AddScoped<IMessageProducer, RabbitMQProducer>();
            return services;
        }
    }
}
