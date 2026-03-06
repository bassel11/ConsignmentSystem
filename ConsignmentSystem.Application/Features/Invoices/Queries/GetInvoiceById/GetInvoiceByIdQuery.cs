using ConsignmentSystem.Application.DTOs.Invoices;
using MediatR;

namespace ConsignmentSystem.Application.Features.Invoices.Queries.GetInvoiceById
{
    public record GetInvoiceByIdQuery(Guid VehicleId, Guid InvoiceId)
        : IRequest<InvoiceResponseDto>;
}
