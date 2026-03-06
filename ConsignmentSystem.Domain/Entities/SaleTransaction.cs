using ConsignmentSystem.Domain.Common;

namespace ConsignmentSystem.Domain.Entities
{
    public class SaleTransaction : AuditableEntity
    {
        public Guid Id { get; private set; }
        public Guid VehicleId { get; private set; }
        public Guid ConsignmentItemId { get; private set; }
        public int Quantity { get; private set; } = default!;
        public decimal SalePrice { get; private set; } = default!;
        public DateTime SaleDate { get; private set; } = default!;
        public bool IsInvoiced { get; private set; }

        protected SaleTransaction() { }

        public SaleTransaction(Guid vehicleId, Guid consignmentItemId, int quantity, decimal salePrice, string createdBy)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
            if (salePrice < 0) throw new ArgumentException("Sale price cannot be negative.");

            Id = Guid.NewGuid();
            VehicleId = vehicleId;
            ConsignmentItemId = consignmentItemId;
            Quantity = quantity;
            SalePrice = salePrice;
            SaleDate = DateTime.UtcNow;
            IsInvoiced = false;
            CreatedBy = createdBy;
        }

        public void MarkAsInvoiced()
        {
            if (IsInvoiced) throw new InvalidOperationException("Transaction is already invoiced.");
            IsInvoiced = true;
        }
    }
}
