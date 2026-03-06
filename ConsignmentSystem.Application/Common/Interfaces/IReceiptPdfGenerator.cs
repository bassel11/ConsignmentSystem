using ConsignmentSystem.Application.DTOs.Consignments;

namespace ConsignmentSystem.Application.Common.Interfaces
{
    public interface IReceiptPdfGenerator
    {
        byte[] GenerateReceiptPdf(ConsignmentReceiptResponseDto receipt);
    }
}
