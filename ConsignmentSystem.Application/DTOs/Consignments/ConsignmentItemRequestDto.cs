using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Application.DTOs.Consignments
{
    public record ConsignmentItemRequestDto(string ProductName, int Quantity, decimal UnitPrice);
}
