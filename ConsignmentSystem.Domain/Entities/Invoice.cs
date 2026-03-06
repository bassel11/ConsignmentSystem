using ConsignmentSystem.Domain.Common;

namespace ConsignmentSystem.Domain.Entities
{
    public class Invoice : AuditableEntity
    {
        public Guid Id { get; private set; }
        public Guid VehicleId { get; private set; }
        public string InvoiceNumber { get; private set; } = default!;
        public decimal TotalSalesAmount { get; private set; } = default!;
        public decimal CommissionAmount { get; private set; } = default!;
        public decimal ExpensesAmount { get; private set; } = default!;
        public decimal NetPayable { get; private set; } = default!;
        public DateTime InvoiceDate { get; private set; } = default!;

        protected Invoice() { }

        public Invoice(Guid vehicleId, string invoiceNumber, decimal totalSalesAmount, decimal commissionAmount, decimal expensesAmount, string createdBy)
        {
            if (totalSalesAmount < 0 || commissionAmount < 0 || expensesAmount < 0)
                throw new ArgumentException("Financial amounts cannot be negative.");

            Id = Guid.NewGuid();
            VehicleId = vehicleId;
            InvoiceNumber = invoiceNumber;
            TotalSalesAmount = totalSalesAmount;
            CommissionAmount = commissionAmount;
            ExpensesAmount = expensesAmount;

            CalculateNetPayable();

            InvoiceDate = DateTime.UtcNow;
            CreatedBy = createdBy;
        }

        private void CalculateNetPayable()
        {
            NetPayable = TotalSalesAmount - CommissionAmount - ExpensesAmount;

            if (NetPayable < 0)
                throw new InvalidOperationException("Net Payable is negative. Check commission and expenses parameters.");
        }
    }
}
