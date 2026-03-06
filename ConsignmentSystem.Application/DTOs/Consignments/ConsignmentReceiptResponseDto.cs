using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Application.DTOs.Consignments
{
    public record ConsignmentReceiptResponseDto(
        Guid Id,
        Guid VehicleId,
        string ReceiptNumber,
        DateTime ReceiptDate,
        IEnumerable<ConsignmentItemResponseDto> Items);
    public record ConsignmentItemResponseDto(
        Guid Id,
        string ProductName,
        int QuantityReceived,
        int QuantitySold,
        decimal UnitPrice);
}
