using CustomerOrderManager.Business.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CustomerOrderManager.Business.Context
{
    public class CustomerOrderManagerDbContext : DbContext, ICustomerOrderManagerDbContext
    {
        public CustomerOrderManagerDbContext(DbContextOptions<CustomerOrderManagerDbContext> options)
            : base(options)
        { }

        public DbSet<CustomerOrder> CustomerOrders { get; set; }
        public DbSet<CustomerOrderParameter> CustomerOrderParameters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
