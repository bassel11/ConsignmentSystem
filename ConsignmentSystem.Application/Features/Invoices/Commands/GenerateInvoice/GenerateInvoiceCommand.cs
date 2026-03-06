using ConsignmentSystem.Application.DTOs.Invoices;
using MediatR;

namespace ConsignmentSystem.Application.Features.Invoices.Commands.GenerateInvoice
{
    public record GenerateInvoiceCommand(
        string InvoiceNumber,
        decimal? OverrideCommissionAmount,
        decimal ExpensesAmount) : IRequest<InvoiceResponseDto>
    {
        public Guid VehicleId { get; set; }
    }
}
