using ConsignmentSystem.Application.Common.Exceptions;
using ConsignmentSystem.Application.Common.Interfaces;
using ConsignmentSystem.Application.Data;
using ConsignmentSystem.Domain.Entities;
using ConsignmentSystem.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Application.Features.Vendors.Commands.DeleteVendor
{
    public class DeleteVendorCommandHandler : IRequestHandler<DeleteVendorCommand>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IApplicationDbContext _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteVendorCommandHandler(
            IVendorRepository vendorRepository,
            IVehicleRepository vehicleRepository,
            IApplicationDbContext unitOfWork,
            ICurrentUserService currentUserService)
        {
            _vendorRepository = vendorRepository;
            _vehicleRepository = vehicleRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task Handle(DeleteVendorCommand request, CancellationToken cancellationToken)
        {
            var vendor = await _vendorRepository.GetByIdAsync(request.Id, cancellationToken);
            if (vendor == null) throw new NotFoundException(nameof(Vendor), request.Id);

            if (await _vehicleRepository.AnyByVendorIdAsync(request.Id, cancellationToken))
            {
                throw new InvalidOperationException("Cannot delete this vendor because they have associated vehicles. Please remove the vehicles first.");
            }

            string currentUser = _currentUserService.Email ?? "Unknown";
            vendor.MarkAsDeleted(currentUser);

            _vendorRepository.Update(vendor);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
