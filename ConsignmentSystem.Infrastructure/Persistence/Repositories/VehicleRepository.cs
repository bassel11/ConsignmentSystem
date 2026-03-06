using ConsignmentSystem.Domain.Entities;
using ConsignmentSystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Infrastructure.Persistence.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        }
        public async Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken)
        {
            await _context.Vehicles.AddAsync(vehicle, cancellationToken);
        }

        public async Task<bool> ExistsByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken)
        {
            return await _context.Vehicles.AnyAsync(v => v.LicensePlate == licensePlate, cancellationToken);
        }

        public async Task<bool> AnyByVendorIdAsync(Guid vendorId, CancellationToken cancellationToken)
        {
            return await _context.Vehicles.AnyAsync(v => v.VendorId == vendorId, cancellationToken);
        }
    }
}
