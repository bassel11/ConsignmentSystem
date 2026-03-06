using ConsignmentSystem.Application.DTOs.Consignments;
using MediatR;

namespace ConsignmentSystem.Application.Features.Consignments.Queries.GetReceiptById
{
    public record GetConsignmentReceiptQuery(Guid VehicleId, Guid ReceiptId)
        : IRequest<ConsignmentReceiptResponseDto>;
}
