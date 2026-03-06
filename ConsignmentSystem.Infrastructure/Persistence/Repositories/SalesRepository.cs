using ConsignmentSystem.Domain.Entities;
using ConsignmentSystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Infrastructure.Persistence.Repositories
{
    public class SalesRepository : ISalesRepository
    {
        private readonly ApplicationDbContext _context;
        public SalesRepository(ApplicationDbContext context) => _context = context;

        public async Task AddAsync(SaleTransaction sale, CancellationToken cancellationToken)
        {
            await _context.SaleTransactions.AddAsync(sale, cancellationToken);
        }
        public async Task<List<SaleTransaction>> GetUninvoicedSalesByVehicleAsync(Guid vehicleId, CancellationToken cancellationToken)
        {
            return await _context.SaleTransactions
                .Where(s => s.VehicleId == vehicleId && !s.IsInvoiced)
                .ToListAsync(cancellationToken);
        }
    }
}
