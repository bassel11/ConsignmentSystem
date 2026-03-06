using ConsignmentSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsignmentSystem.Infrastructure.Persistence.Configurations
{
    public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.ToTable("Vendors");
            builder.HasKey(v => v.Id);
            builder.Property(v => v.Name).IsRequired().HasMaxLength(200);
            builder.Property(v => v.ContactEmail).IsRequired().HasMaxLength(150);

            builder.Property(v => v.DefaultCommissionPercentage).HasColumnType("decimal(18,2)");

            builder.HasQueryFilter(v => !v.IsDeleted);
        }
    }
}
