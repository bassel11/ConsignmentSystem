using ConsignmentSystem.Application.Common.Exceptions;
using ConsignmentSystem.Application.Common.Interfaces;
using ConsignmentSystem.Application.Data;
using ConsignmentSystem.Domain.Entities;
using ConsignmentSystem.Domain.Repositories;
using MediatR;

namespace ConsignmentSystem.Application.Features.Vendors.Commands.UpdateVendor
{
    public class UpdateVendorCommandHandler : IRequestHandler<UpdateVendorCommand>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IApplicationDbContext _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateVendorCommandHandler(IVendorRepository vendorRepository, IApplicationDbContext unitOfWork, ICurrentUserService currentUserService)
        {
            _vendorRepository = vendorRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
        {
            var vendor = await _vendorRepository.GetByIdAsync(request.Id, cancellationToken);
            if (vendor == null) throw new NotFoundException(nameof(Vendor), request.Id);

            if (vendor.ContactEmail != request.ContactEmail && await _vendorRepository.ExistsByEmailAsync(request.ContactEmail, cancellationToken))
            {
                throw new InvalidOperationException($"Email {request.ContactEmail} is already in use by another vendor.");
            }

            string currentUser = _currentUserService.Email ?? "Unknown";
            vendor.UpdateDetails(request.Name, request.ContactEmail, request.DefaultCommissionPercentage, currentUser);

            _vendorRepository.Update(vendor);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
