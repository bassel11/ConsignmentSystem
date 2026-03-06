using ConsignmentSystem.Application.DTOs.Vendors;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Application.Features.Vendors.Queries.GetVendorById
{
    public record GetVendorByIdQuery(Guid Id) : IRequest<VendorResponseDto>;
}
