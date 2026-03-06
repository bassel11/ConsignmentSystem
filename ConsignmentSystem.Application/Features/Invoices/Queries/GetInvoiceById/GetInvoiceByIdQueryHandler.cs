using ConsignmentSystem.Application.Common.Exceptions;
using ConsignmentSystem.Application.Data;
using ConsignmentSystem.Application.DTOs.Invoices;
using ConsignmentSystem.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConsignmentSystem.Application.Features.Invoices.Queries.GetInvoiceById
{
    public class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, InvoiceResponseDto>
    {
        private readonly IApplicationDbContext _context;

        public GetInvoiceByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<InvoiceResponseDto> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
        {
            var invoice = await _context.Invoices
                .AsNoTracking()
                .Where(i => i.Id == request.InvoiceId && i.VehicleId == request.VehicleId)
                .ProjectToType<InvoiceResponseDto>()
                .FirstOrDefaultAsync(cancellationToken);

            if (invoice == null)
            {
                throw new NotFoundException(
                    nameof(Invoice),
                    $"InvoiceId: {request.InvoiceId}, VehicleId: {request.VehicleId}");
            }

            return invoice;
        }
    }
}
