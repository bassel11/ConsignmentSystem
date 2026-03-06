using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Application.DTOs.Invoices
{
    public record InvoiceResponseDto(
        Guid Id,
        Guid VehicleId,
        string InvoiceNumber,
        decimal TotalSalesAmount,
        decimal CommissionAmount,
        decimal ExpensesAmount,
        decimal NetPayable,
        DateTime InvoiceDate);
}
