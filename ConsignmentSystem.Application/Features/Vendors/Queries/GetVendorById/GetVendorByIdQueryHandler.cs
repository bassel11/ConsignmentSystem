using ConsignmentSystem.Application.Common.Exceptions;
using ConsignmentSystem.Application.Data;
using ConsignmentSystem.Application.DTOs.Vendors;
using ConsignmentSystem.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Application.Features.Vendors.Queries.GetVendorById
{
    public class GetVendorByIdQueryHandler : IRequestHandler<GetVendorByIdQuery, VendorResponseDto>
    {
        private readonly IApplicationDbContext _context;

        public GetVendorByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VendorResponseDto> Handle(GetVendorByIdQuery request, CancellationToken cancellationToken)
        {
            var vendorDto = await _context.Vendors
                .AsNoTracking()
                .Where(v => v.Id == request.Id)
                .ProjectToType<VendorResponseDto>()
                .FirstOrDefaultAsync(cancellationToken);

            if (vendorDto == null)
            {
                throw new NotFoundException(nameof(Vendor), request.Id);
            }

            return vendorDto;
        }
    }
}
