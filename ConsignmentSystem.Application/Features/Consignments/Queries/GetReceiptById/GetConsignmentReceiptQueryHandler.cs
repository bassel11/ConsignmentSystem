using ConsignmentSystem.Application.Common.Exceptions;
using ConsignmentSystem.Application.Data;
using ConsignmentSystem.Application.DTOs.Consignments;
using ConsignmentSystem.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConsignmentSystem.Application.Features.Consignments.Queries.GetReceiptById
{
    public class GetConsignmentReceiptQueryHandler : IRequestHandler<GetConsignmentReceiptQuery, ConsignmentReceiptResponseDto>
    {
        private readonly IApplicationDbContext _context;

        public GetConsignmentReceiptQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ConsignmentReceiptResponseDto> Handle(GetConsignmentReceiptQuery request, CancellationToken cancellationToken)
        {
            var receipt = await _context.ConsignmentReceipts
                .AsNoTracking()
                .Where(r => r.Id == request.ReceiptId && r.VehicleId == request.VehicleId)
                .ProjectToType<ConsignmentReceiptResponseDto>()
                .FirstOrDefaultAsync(cancellationToken);

            if (receipt == null)
            {
                throw new NotFoundException(
                    nameof(ConsignmentReceipt),
                    $"ReceiptId: {request.ReceiptId}, VehicleId: {request.VehicleId}");
            }

            return receipt;
        }
    }
}
