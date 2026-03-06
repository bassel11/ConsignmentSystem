using ConsignmentSystem.Application.Common.Interfaces;
using ConsignmentSystem.Application.Data;
using ConsignmentSystem.Application.DTOs.Vendors;
using ConsignmentSystem.Domain.Entities;
using ConsignmentSystem.Domain.Repositories;
using Mapster;
using MediatR;

namespace ConsignmentSystem.Application.Features.Vendors.Commands.CreateVendor
{
    public class CreateVendorCommandHandler : IRequestHandler<CreateVendorCommand, VendorResponseDto>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IApplicationDbContext _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateVendorCommandHandler(
            IVendorRepository vendorRepository,
            IApplicationDbContext unitOfWork,
            ICurrentUserService currentUserService)
        {
            _vendorRepository = vendorRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<VendorResponseDto> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
        {
            if (await _vendorRepository.ExistsByEmailAsync(request.ContactEmail, cancellationToken))
            {
                throw new InvalidOperationException($"Vendor with email {request.ContactEmail} already exists.");
            }

            string currentUser = _currentUserService.Email ?? "Unknown";
            var vendor = new Vendor(request.Name, request.ContactEmail, currentUser, request.DefaultCommissionPercentage);

            await _vendorRepository.AddAsync(vendor, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return vendor.Adapt<VendorResponseDto>();
        }
    }
}