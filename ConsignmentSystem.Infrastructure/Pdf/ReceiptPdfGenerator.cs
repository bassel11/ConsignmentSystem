using ConsignmentSystem.Application.Common.Interfaces;
using ConsignmentSystem.Application.DTOs.Consignments;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ConsignmentSystem.Infrastructure.Pdf
{
    public class ReceiptPdfGenerator : IReceiptPdfGenerator
    {
        public ReceiptPdfGenerator()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public byte[] GenerateReceiptPdf(ConsignmentReceiptResponseDto receipt)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    page.Header().Element(header => ComposeHeader(header, receipt));
                    page.Content().Element(content => ComposeContent(content, receipt));
                    page.Footer().Element(ComposeFooter);
                });
            });

            return document.GeneratePdf();
        }

        private void ComposeHeader(IContainer container, ConsignmentReceiptResponseDto receipt)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("CONSIGNMENT RECEIPT").FontSize(24).SemiBold().FontColor(Colors.Blue.Darken2);
                    column.Item().Text($"Receipt Number: {receipt.ReceiptNumber}").FontSize(14).SemiBold();
                    column.Item().Text($"Date: {receipt.ReceiptDate:dd MMM yyyy HH:mm}");
                    column.Item().Text($"Vehicle ID: {receipt.VehicleId}").FontColor(Colors.Grey.Medium);
                });
            });
        }

        private void ComposeContent(IContainer container, ConsignmentReceiptResponseDto receipt)
        {
            container.PaddingVertical(1, Unit.Centimetre).Column(column =>
            {
                column.Item().PaddingBottom(5).Text("Received Goods:").FontSize(14).SemiBold();

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(30);   // #
                        columns.RelativeColumn();     // Product Name
                        columns.ConstantColumn(80);   // Quantity
                        columns.ConstantColumn(80);   // Unit Price
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("#");
                        header.Cell().Element(CellStyle).Text("Product Name");
                        header.Cell().Element(CellStyle).AlignRight().Text("Quantity");
                        header.Cell().Element(CellStyle).AlignRight().Text("Unit Price");

                        static IContainer CellStyle(IContainer container)
                        {
                            return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                        }
                    });

                    var index = 1;
                    foreach (var item in receipt.Items)
                    {
                        table.Cell().Element(CellStyle).Text(index.ToString());
                        table.Cell().Element(CellStyle).Text(item.ProductName);

                        table.Cell().Element(CellStyle).AlignRight().Text(item.QuantityReceived.ToString());

                        table.Cell().Element(CellStyle).AlignRight().Text($"${item.UnitPrice:F2}");
                        index++;

                        static IContainer CellStyle(IContainer container)
                        {
                            return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                        }
                    }
                });

                column.Item().PaddingTop(15).AlignRight().Text($"Total Items Types: {receipt.Items.Count()}").SemiBold();

                column.Item().AlignRight().Text($"Total Quantity: {receipt.Items.Sum(x => x.QuantityReceived)}").SemiBold();
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.AlignCenter().Text(x =>
            {
                x.Span("Page ");
                x.CurrentPageNumber();
                x.Span(" of ");
                x.TotalPages();
            });
        }
    }
}