using CustomerOrderManager.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CustomerOrderManager.Business.Context
{
    public interface ICustomerOrderManagerDbContext
    {
        DbSet<CustomerOrder> CustomerOrders { get; set; }
        DbSet<CustomerOrderParameter> CustomerOrderParameters { get; set; }
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
