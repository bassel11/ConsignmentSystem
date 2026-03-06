using ConsignmentSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Application.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Vendor> Vendors { get; }
        DbSet<Vehicle> Vehicles { get; }
        DbSet<ConsignmentReceipt> ConsignmentReceipts { get; }
        DbSet<ConsignmentItem> ConsignmentItems { get; }
        DbSet<SaleTransaction> SaleTransactions { get; }
        DbSet<Invoice> Invoices { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
