using ConsignmentSystem.Application.Common.Exceptions;
using ConsignmentSystem.Application.Common.Interfaces;
using ConsignmentSystem.Application.Data;
using ConsignmentSystem.Application.DTOs.Consignments;
using ConsignmentSystem.Application.Features.Consignments.Commands.CreateReceipt.ConsignmentSystem.Application.Features.Consignments.Commands.CreateReceipt;
using ConsignmentSystem.Domain.Entities;
using ConsignmentSystem.Domain.Repositories;
using Mapster;
using MediatR;

namespace ConsignmentSystem.Application.Features.Consignments.Commands.CreateReceipt
{
    public class CreateConsignmentReceiptCommandHandler : IRequestHandler<CreateConsignmentReceiptCommand, ConsignmentReceiptResponseDto>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IConsignmentRepository _consignmentRepository;
        private readonly IApplicationDbContext _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateConsignmentReceiptCommandHandler(
            IVehicleRepository vehicleRepository,
            IConsignmentRepository consignmentRepository,
            IApplicationDbContext unitOfWork,
            ICurrentUserService currentUserService)
        {
            _vehicleRepository = vehicleRepository;
            _consignmentRepository = consignmentRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ConsignmentReceiptResponseDto> Handle(CreateConsignmentReceiptCommand request, CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken);
            if (vehicle == null)
            {
                throw new NotFoundException(nameof(Vehicle), request.VehicleId);
            }

            if (await _consignmentRepository.ExistsByReceiptNumberAsync(request.ReceiptNumber, cancellationToken))
            {
                throw new InvalidOperationException($"Receipt number '{request.ReceiptNumber}' is already registered.");
            }

            string currentUser = _currentUserService.Email ?? "Unknown";

            var receipt = new ConsignmentReceipt(request.VehicleId, request.ReceiptNumber, currentUser);

            foreach (var itemDto in request.Items)
            {
                receipt.AddItem(itemDto.ProductName, itemDto.Quantity, itemDto.UnitPrice);
            }

            await _consignmentRepository.AddAsync(receipt, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return receipt.Adapt<ConsignmentReceiptResponseDto>();
        }
    }
}
