using ConsignmentSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Domain.Repositories
{
    public interface IConsignmentRepository
    {
        Task<ConsignmentReceipt?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(ConsignmentReceipt receipt, CancellationToken cancellationToken);
        Task<bool> ExistsByReceiptNumberAsync(string receiptNumber, CancellationToken cancellationToken);
    }
}
