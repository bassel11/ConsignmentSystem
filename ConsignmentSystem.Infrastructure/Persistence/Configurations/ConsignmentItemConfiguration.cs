using ConsignmentSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsignmentSystem.Infrastructure.Persistence.Configurations
{
    public class ConsignmentItemConfiguration : IEntityTypeConfiguration<ConsignmentItem>
    {
        public void Configure(EntityTypeBuilder<ConsignmentItem> builder)
        {
            builder.ToTable("ConsignmentItems");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.ProductName).IsRequired().HasMaxLength(250);

            builder.Property(c => c.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();

            builder.HasOne<ConsignmentReceipt>()
                   .WithMany(r => r.Items)
                   .HasForeignKey(c => c.ConsignmentReceiptId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(c => !c.IsDeleted);
        }
    }
}
