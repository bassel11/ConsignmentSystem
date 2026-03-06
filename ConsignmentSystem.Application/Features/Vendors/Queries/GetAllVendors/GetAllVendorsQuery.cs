using ConsignmentSystem.Application.DTOs.Vendors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Application.Features.Vendors.Queries.GetAllVendors
{
    public record GetAllVendorsQuery() : IRequest<IEnumerable<VendorResponseDto>>;
}
