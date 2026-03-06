using ConsignmentSystem.Domain.Entities;
using ConsignmentSystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Infrastructure.Persistence.Repositories
{
    public class ConsignmentItemRepository : IConsignmentItemRepository
    {
        private readonly ApplicationDbContext _context;
        public ConsignmentItemRepository(ApplicationDbContext context) => _context = context;

        public async Task<ConsignmentItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.ConsignmentItems.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }
    }
}
