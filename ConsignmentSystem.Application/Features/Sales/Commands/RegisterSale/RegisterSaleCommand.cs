using ConsignmentSystem.Application.DTOs.Sales;
using MediatR;

namespace ConsignmentSystem.Application.Features.Sales.Commands.RegisterSale
{
    public record RegisterSaleCommand(
        Guid ConsignmentItemId,
        int Quantity,
        decimal SalePrice) : IRequest<SaleResponseDto>
    {
        public Guid VehicleId { get; set; }
    }
}
