using ConsignmentSystem.Application.Common.Exceptions;
using ConsignmentSystem.Application.Common.Interfaces;
using ConsignmentSystem.Application.Data;
using ConsignmentSystem.Application.DTOs.Invoices;
using ConsignmentSystem.Domain.Entities;
using ConsignmentSystem.Domain.Repositories;
using Mapster;
using MediatR;

namespace ConsignmentSystem.Application.Features.Invoices.Commands.GenerateInvoice
{
    public class GenerateInvoiceCommandHandler : IRequestHandler<GenerateInvoiceCommand, InvoiceResponseDto>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IVendorRepository _vendorRepository;
        private readonly ISalesRepository _salesRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IApplicationDbContext _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GenerateInvoiceCommandHandler(
            IVehicleRepository vehicleRepository,
            IVendorRepository vendorRepository,
            ISalesRepository salesRepository,
            IInvoiceRepository invoiceRepository,
            IApplicationDbContext unitOfWork,
            ICurrentUserService currentUserService)
        {
            _vehicleRepository = vehicleRepository;
            _vendorRepository = vendorRepository;
            _salesRepository = salesRepository;
            _invoiceRepository = invoiceRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<InvoiceResponseDto> Handle(GenerateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken);
            if (vehicle == null) throw new NotFoundException(nameof(Vehicle), request.VehicleId);

            var vendor = await _vendorRepository.GetByIdAsync(vehicle.VendorId, cancellationToken);
            if (vendor == null) throw new InvalidOperationException("Critical Data Error: Vehicle has no valid vendor.");

            var pendingSales = await _salesRepository.GetUninvoicedSalesByVehicleAsync(request.VehicleId, cancellationToken);
            if (!pendingSales.Any())
            {
                throw new InvalidOperationException("No pending un-invoiced sales found for this vehicle to generate an invoice.");
            }

            decimal totalSalesAmount = pendingSales.Sum(s => s.SalePrice * s.Quantity);

            decimal finalCommissionAmount = request.OverrideCommissionAmount
                ?? (totalSalesAmount * (vendor.DefaultCommissionPercentage / 100m));

            string currentUser = _currentUserService.Email ?? "Unknown";

            var invoice = new Invoice(
                request.VehicleId,
                request.InvoiceNumber,
                totalSalesAmount,
                finalCommissionAmount,
                request.ExpensesAmount,
                currentUser);

            foreach (var sale in pendingSales)
            {
                sale.MarkAsInvoiced();
            }

            await _invoiceRepository.AddAsync(invoice, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return invoice.Adapt<InvoiceResponseDto>();
        }
    }
}