using ConsignmentSystem.Application.Common.Constants;
using ConsignmentSystem.Application.Common.Interfaces;
using ConsignmentSystem.Application.DTOs.Consignments;
using ConsignmentSystem.Application.DTOs.Invoices;
using ConsignmentSystem.Application.DTOs.Sales;
using ConsignmentSystem.Application.Features.Consignments.Commands.CreateReceipt.ConsignmentSystem.Application.Features.Consignments.Commands.CreateReceipt;
using ConsignmentSystem.Application.Features.Consignments.Queries.GetReceiptById;
using ConsignmentSystem.Application.Features.Invoices.Commands.GenerateInvoice;
using ConsignmentSystem.Application.Features.Invoices.Queries.GetInvoiceById;
using ConsignmentSystem.Application.Features.Sales.Commands.RegisterSale;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConsignmentSystem.API.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class VehiclesController : ControllerBase
    {
        private readonly ISender _mediator;
        private readonly IReceiptPdfGenerator _pdfGenerator;

        public VehiclesController(ISender mediator, IReceiptPdfGenerator pdfGenerator)
        {
            _mediator = mediator;
            _pdfGenerator = pdfGenerator;
        }


        [HttpPost("{vehicleId:guid}/consignments")]
        [Authorize(Roles = AppRoles.Admin + "," + AppRoles.Storekeeper)]
        [ProducesResponseType(typeof(ConsignmentReceiptResponseDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateConsignmentReceipt(Guid vehicleId, [FromBody] CreateConsignmentReceiptCommand command)
        {
            command.VehicleId = vehicleId;
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetConsignmentReceipt), new { vehicleId = vehicleId, receiptId = result.Id }, result);
        }

        [HttpGet("{vehicleId:guid}/consignments/{receiptId:guid}", Name = "GetConsignmentReceipt")]
        [Authorize(Roles = AppRoles.Admin + "," + AppRoles.Storekeeper)]
        [ProducesResponseType(typeof(ConsignmentReceiptResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetConsignmentReceipt(Guid vehicleId, Guid receiptId)
        {
            var result = await _mediator.Send(new GetConsignmentReceiptQuery(vehicleId, receiptId));
            return Ok(result);
        }

        [HttpGet("{vehicleId:guid}/consignments/{receiptId:guid}/pdf")]
        [Authorize(Roles = AppRoles.Admin + "," + AppRoles.Storekeeper)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadReceiptPdf(Guid vehicleId, Guid receiptId)
        {
            var receipt = await _mediator.Send(new GetConsignmentReceiptQuery(vehicleId, receiptId));
            var pdfBytes = _pdfGenerator.GenerateReceiptPdf(receipt);
            return File(pdfBytes, "application/pdf", $"Receipt_{receipt.ReceiptNumber}.pdf");
        }


        [HttpPost("{vehicleId:guid}/sales")]
        [Authorize(Roles = AppRoles.Admin + "," + AppRoles.Accountant)]
        [ProducesResponseType(typeof(SaleResponseDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> RegisterSale(Guid vehicleId, [FromBody] RegisterSaleCommand command)
        {
            command.VehicleId = vehicleId;
            var result = await _mediator.Send(command);
            return Created(string.Empty, result);
        }

        [HttpPost("{vehicleId:guid}/invoices")]
        [Authorize(Roles = AppRoles.Admin + "," + AppRoles.Accountant)]
        [ProducesResponseType(typeof(InvoiceResponseDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> GenerateInvoice(Guid vehicleId, [FromBody] GenerateInvoiceCommand command)
        {
            command.VehicleId = vehicleId;
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetInvoice), new { vehicleId = vehicleId, invoiceId = result.Id }, result);
        }

        [HttpGet("{vehicleId:guid}/invoices/{invoiceId:guid}", Name = "GetInvoice")]
        [Authorize(Roles = AppRoles.Admin + "," + AppRoles.Accountant)]
        [ProducesResponseType(typeof(InvoiceResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetInvoice(Guid vehicleId, Guid invoiceId)
        {
            var result = await _mediator.Send(new GetInvoiceByIdQuery(vehicleId, invoiceId));
            return Ok(result);
        }
    }
}