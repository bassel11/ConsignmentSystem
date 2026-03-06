using ConsignmentSystem.Application.Data;
using ConsignmentSystem.Application.DTOs.Vendors;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Application.Features.Vendors.Queries.GetAllVendors
{
    public class GetAllVendorsQueryHandler : IRequestHandler<GetAllVendorsQuery, IEnumerable<VendorResponseDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllVendorsQueryHandler(IApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<VendorResponseDto>> Handle(GetAllVendorsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Vendors
                .AsNoTracking() 
                .ProjectToType<VendorResponseDto>()
                .ToListAsync(cancellationToken);
        }
    }
}
