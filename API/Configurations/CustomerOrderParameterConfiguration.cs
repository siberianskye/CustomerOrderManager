using CustomerOrderManager.Business.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerOrderManager.Business.Configurations
{
    public class CustomerOrderParameterConfiguration : IEntityTypeConfiguration<CustomerOrderParameter>
    {
        public void Configure(EntityTypeBuilder<CustomerOrderParameter> builder)
        {
            builder.ToTable("CustomerOrderParameter", "CustomerOrderManager");

            builder.Property(customerOrder => customerOrder.CustomerOrderParameter_UID)
                .HasDefaultValueSql("newId()");
        }
    }
}
