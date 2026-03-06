using ConsignmentSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsignmentSystem.Infrastructure.Persistence.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoices");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.InvoiceNumber).IsRequired().HasMaxLength(50);

            builder.Property(i => i.TotalSalesAmount).HasColumnType("decimal(18,2)");
            builder.Property(i => i.CommissionAmount).HasColumnType("decimal(18,2)");
            builder.Property(i => i.ExpensesAmount).HasColumnType("decimal(18,2)");
            builder.Property(i => i.NetPayable).HasColumnType("decimal(18,2)");

            builder.HasOne<Vehicle>()
                   .WithMany()
                   .HasForeignKey(i => i.VehicleId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(i => !i.IsDeleted);
        }
    }
}
