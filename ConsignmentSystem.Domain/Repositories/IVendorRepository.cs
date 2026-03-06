using ConsignmentSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Domain.Repositories
{
    public interface IVendorRepository
    {
        Task<Vendor?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken);
        Task AddAsync(Vendor vendor, CancellationToken cancellationToken);
        void Update(Vendor vendor);
    }
}
