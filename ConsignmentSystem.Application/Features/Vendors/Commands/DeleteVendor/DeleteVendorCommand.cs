using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Application.Features.Vendors.Commands.DeleteVendor
{
    public record DeleteVendorCommand(Guid Id) : IRequest;
}

