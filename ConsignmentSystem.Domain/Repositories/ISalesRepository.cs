using ConsignmentSystem.Domain.Entities;

namespace ConsignmentSystem.Domain.Repositories
{
    public interface ISalesRepository
    {
        Task AddAsync(SaleTransaction sale, CancellationToken cancellationToken);
        Task<List<SaleTransaction>> GetUninvoicedSalesByVehicleAsync(Guid vehicleId, CancellationToken cancellationToken);
    }
}
