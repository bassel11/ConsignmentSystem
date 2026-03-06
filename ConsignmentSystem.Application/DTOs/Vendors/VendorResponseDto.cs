using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Application.DTOs.Vendors
{
    public record VendorResponseDto(
        Guid Id,
        string Name,
        string ContactEmail,
        DateTime CreatedAt);
}
