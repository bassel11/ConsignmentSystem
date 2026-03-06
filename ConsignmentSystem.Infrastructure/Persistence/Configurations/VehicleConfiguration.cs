using ConsignmentSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsignmentSystem.Infrastructure.Persistence.Configurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.ToTable("Vehicles");
            builder.HasKey(v => v.Id);
            builder.Property(v => v.LicensePlate).IsRequired().HasMaxLength(50);
            builder.Property(v => v.DriverName).IsRequired().HasMaxLength(200);

            builder.HasOne<Vendor>()
                   .WithMany(v => v.Vehicles)
                   .HasForeignKey(v => v.VendorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(v => !v.IsDeleted);
        }
    }
}
