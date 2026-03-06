using ConsignmentSystem.Domain.Entities;
using ConsignmentSystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ConsignmentSystem.Infrastructure.Persistence.Repositories
{
    public class VendorRepository : IVendorRepository
    {
        private readonly ApplicationDbContext _context;

        public VendorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Vendor?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Vendors.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Vendors.AnyAsync(v => v.ContactEmail == email, cancellationToken);
        }

        public async Task AddAsync(Vendor vendor, CancellationToken cancellationToken)
        {
            await _context.Vendors.AddAsync(vendor, cancellationToken);
        }

        public void Update(Vendor vendor)
        {
            _context.Vendors.Update(vendor);
        }
    }
}
