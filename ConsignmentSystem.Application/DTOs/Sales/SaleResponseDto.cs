using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Application.DTOs.Sales
{
    public record SaleResponseDto(
        Guid Id,
        Guid VehicleId,
        Guid ConsignmentItemId,
        int Quantity,
        decimal SalePrice,
        DateTime SaleDate,
        bool IsInvoiced);
}
