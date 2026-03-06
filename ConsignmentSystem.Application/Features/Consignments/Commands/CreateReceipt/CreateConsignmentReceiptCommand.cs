namespace ConsignmentSystem.Application.Features.Consignments.Commands.CreateReceipt
{
    using global::ConsignmentSystem.Application.DTOs.Consignments;
    using MediatR;
    using System;
    using System.Collections.Generic;

    namespace ConsignmentSystem.Application.Features.Consignments.Commands.CreateReceipt
    {
        public record CreateConsignmentReceiptCommand(
            string ReceiptNumber,
            List<ConsignmentItemRequestDto> Items) : IRequest<ConsignmentReceiptResponseDto>
        {
            public Guid VehicleId { get; set; }
        }
    }
}
