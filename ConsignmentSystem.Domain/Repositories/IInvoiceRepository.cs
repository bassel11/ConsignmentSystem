using ConsignmentSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Domain.Repositories
{
    public interface IInvoiceRepository
    {
        Task AddAsync(Invoice invoice, CancellationToken cancellationToken);
    }
}
