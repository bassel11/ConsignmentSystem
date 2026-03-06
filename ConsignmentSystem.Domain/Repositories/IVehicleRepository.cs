using ConsignmentSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Domain.Repositories
{
    public interface IVehicleRepository
    {
        Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(Vehicle vehicle, CancellationToken cancellationToken);
        Task<bool> ExistsByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken);
        Task<bool> AnyByVendorIdAsync(Guid vendorId, CancellationToken cancellationToken);
    }
}
