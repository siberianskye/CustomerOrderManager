using CustomerOrderManager.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerOrderManager.Business.Configurations
{
    public class CustomerOrderConfiguration : IEntityTypeConfiguration<CustomerOrder>
    {
        public void Configure(EntityTypeBuilder<CustomerOrder> builder)
        {
            builder.ToTable("CustomerOrder", "CustomerOrderManager");

            builder.Property(customerOrder => customerOrder.CustomerName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(customerOrder => customerOrder.Description)
                .HasMaxLength(1000);

            builder.Property(customerOrder => customerOrder.CustomerOrder_UID)
                .HasDefaultValueSql("newId()");
        }
    }
}
