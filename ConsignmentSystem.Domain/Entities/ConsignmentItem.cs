using ConsignmentSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Domain.Entities
{
    public class ConsignmentItem : AuditableEntity
    {
        public Guid Id { get; private set; }
        public Guid ConsignmentReceiptId { get; private set; }
        public string ProductName { get; private set; } = default!;
        public int QuantityReceived { get; private set; } = default!;
        public int QuantitySold { get; private set; } = default!;
        public decimal UnitPrice { get; private set; } = default!;

        protected ConsignmentItem() { }

        internal ConsignmentItem(Guid receiptId, string productName, int quantityReceived, decimal unitPrice)
        {
            Id = Guid.NewGuid();
            ConsignmentReceiptId = receiptId;
            ProductName = productName;
            QuantityReceived = quantityReceived;
            UnitPrice = unitPrice;
            QuantitySold = 0;
        }

        public void RecordSale(int quantityToSell)
        {
            if (quantityToSell <= 0)
                throw new InvalidOperationException("Sale quantity must be positive.");

            if (QuantitySold + quantityToSell > QuantityReceived)
                throw new InvalidOperationException($"Cannot sell {quantityToSell}. Only {QuantityReceived - QuantitySold} items remaining.");

            QuantitySold += quantityToSell;
        }
    }
}
