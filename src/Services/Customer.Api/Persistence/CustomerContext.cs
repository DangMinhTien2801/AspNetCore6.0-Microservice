using Microsoft.EntityFrameworkCore;

namespace Customer.Api.Persistence
{
    public class CustomerContext : DbContext
    {
        public DbSet<Entities.Customer> Customers { get; set; } = null!;
        public CustomerContext(DbContextOptions<CustomerContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Entities.Customer>().HasIndex(c => c.UserName).IsUnique();
            modelBuilder.Entity<Entities.Customer>().HasIndex(c => c.EmailAddress).IsUnique();
        }

    }
}
