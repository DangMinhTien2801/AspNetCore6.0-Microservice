using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        private readonly ILogger _logger;
        private readonly OrderContext _context;

        public OrderContextSeed(ILogger logger, OrderContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task InitialiseAsync()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "An error occurred while initialising the database");
                throw;
            }
        }
        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while initialising the database");
                throw;
            }
        }
        public async Task TrySeedAsync()
        {
            if(!_context.Orders.Any())
            {
                await _context.Orders.AddRangeAsync(new List<Order>
                {
                    new Order
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = "customer1",
                        FirstName = "Customer",
                        LastName = "Cus1",
                        EmailAddress = "customer1@gmail.com",
                        ShippingAddress = "Wollongong",
                        InvoiceAddress = "Úc",
                        TotalPrice = 250
                    }
                });
            }
        }
    }
}
