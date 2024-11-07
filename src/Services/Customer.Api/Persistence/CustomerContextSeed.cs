using Microsoft.EntityFrameworkCore;

namespace Customer.Api.Persistence
{
    public static class CustomerContextSeed
    {
        public static IHost SeedCustomerData(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var customerContext = scope.ServiceProvider
                .GetRequiredService<CustomerContext>();
            customerContext.Database.MigrateAsync().GetAwaiter().GetResult();
            if (!customerContext.Customers.Any())
            {
                CreateCustomer(customerContext, "customer1", "customer1", "customer1", "customer1@local.com").GetAwaiter().GetResult();
                CreateCustomer(customerContext, "customer2", "customer2", "customer2", "customer2@local.com").GetAwaiter().GetResult();                
            }

            return host;
        }
        private static async Task CreateCustomer(CustomerContext customerContext,
            string userName, string firstName, string lastName, string email)
        {          
            var customer = await customerContext.Customers.
                SingleOrDefaultAsync(c => c.UserName == userName ||
                c.EmailAddress == email);
            if(customer == null)
            {
                var newCustomer = new Entities.Customer
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = userName,
                    FirstName = firstName,
                    LastName = lastName,
                    EmailAddress = email
                };
                await customerContext.Customers.AddAsync(newCustomer);
                await customerContext.SaveChangesAsync();
            }
        }
    }
}
