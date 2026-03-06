using ConsignmentSystem.Application.Common.Exceptions;
using ConsignmentSystem.Application.Common.Interfaces;
using ConsignmentSystem.Application.Data;
using ConsignmentSystem.Application.DTOs.Sales;
using ConsignmentSystem.Domain.Entities;
using ConsignmentSystem.Domain.Repositories;
using Mapster;
using MediatR;

namespace ConsignmentSystem.Application.Features.Sales.Commands.RegisterSale
{
    public class RegisterSaleCommandHandler : IRequestHandler<RegisterSaleCommand, SaleResponseDto>
    {
        private readonly IConsignmentItemRepository _itemRepository;
        private readonly IConsignmentRepository _receiptRepository;
        private readonly ISalesRepository _salesRepository;
        private readonly IApplicationDbContext _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public RegisterSaleCommandHandler(
            IConsignmentItemRepository itemRepository,
            IConsignmentRepository receiptRepository,
            ISalesRepository salesRepository,
            IApplicationDbContext unitOfWork,
            ICurrentUserService currentUserService)
        {
            _itemRepository = itemRepository;
            _receiptRepository = receiptRepository;
            _salesRepository = salesRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<SaleResponseDto> Handle(RegisterSaleCommand request, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetByIdAsync(request.ConsignmentItemId, cancellationToken);
            if (item == null) throw new NotFoundException(nameof(ConsignmentItem), request.ConsignmentItemId);

            var receipt = await _receiptRepository.GetByIdAsync(item.ConsignmentReceiptId, cancellationToken);
            if (receipt == null) throw new InvalidOperationException("Critical Data Error: Consignment item has no valid receipt.");

            if (receipt.VehicleId != request.VehicleId)
            {
                throw new InvalidOperationException("Security Violation: This consignment item does not belong to the specified vehicle.");
            }

            item.RecordSale(request.Quantity);

            string currentUser = _currentUserService.Email ?? "Unknown";

            var saleTransaction = new SaleTransaction(
                request.VehicleId,
                item.Id,
                request.Quantity,
                request.SalePrice,
                currentUser);

            await _salesRepository.AddAsync(saleTransaction, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return saleTransaction.Adapt<SaleResponseDto>();
        }
    }
}
