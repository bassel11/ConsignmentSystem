using ConsignmentSystem.Domain.Entities;
using ConsignmentSystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Infrastructure.Persistence.Repositories
{
    public class ConsignmentRepository : IConsignmentRepository
    {
        private readonly ApplicationDbContext _context;

        public ConsignmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ConsignmentReceipt?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.ConsignmentReceipts.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }
        public async Task AddAsync(ConsignmentReceipt receipt, CancellationToken cancellationToken)
        {
            await _context.ConsignmentReceipts.AddAsync(receipt, cancellationToken);
        }

        public async Task<bool> ExistsByReceiptNumberAsync(string receiptNumber, CancellationToken cancellationToken)
        {
            return await _context.ConsignmentReceipts.AnyAsync(r => r.ReceiptNumber == receiptNumber, cancellationToken);
        }
    }
}
