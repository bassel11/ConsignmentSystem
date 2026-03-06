using ConsignmentSystem.Domain.Entities;
using ConsignmentSystem.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Infrastructure.Persistence.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;
        public InvoiceRepository(ApplicationDbContext context) => _context = context;

        public async Task AddAsync(Invoice invoice, CancellationToken cancellationToken)
        {
            await _context.Invoices.AddAsync(invoice, cancellationToken);
        }
    }
}
