using ConsignmentSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Domain.Entities
{
    public class ConsignmentReceipt : AuditableEntity
    {
        public Guid Id { get; private set; }
        public Guid VehicleId { get; private set; }
        public string ReceiptNumber { get; private set; } = default!;
        public DateTime ReceiptDate { get; private set; } = default!;

        private readonly List<ConsignmentItem> _items = new();
        public IReadOnlyCollection<ConsignmentItem> Items => _items.AsReadOnly();

        protected ConsignmentReceipt() { }

        public ConsignmentReceipt(Guid vehicleId, string receiptNumber, string createdBy)
        {
            Id = Guid.NewGuid();
            VehicleId = vehicleId;
            ReceiptNumber = receiptNumber;
            ReceiptDate = DateTime.UtcNow;
            CreatedBy = createdBy;
        }

        public void AddItem(string productName, int quantity, decimal unitPrice)
        {
            if (quantity <= 0) 
                throw new ArgumentException("Quantity must be greater than zero.");
            if (unitPrice < 0) 
                throw new ArgumentException("Unit price cannot be negative.");

            var item = new ConsignmentItem(Id, productName, quantity, unitPrice);
            _items.Add(item);
        }
    }
}
