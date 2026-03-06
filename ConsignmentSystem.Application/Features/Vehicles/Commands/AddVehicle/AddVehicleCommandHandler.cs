using ConsignmentSystem.Application.Common.Exceptions;
using ConsignmentSystem.Application.Common.Interfaces;
using ConsignmentSystem.Application.Data;
using ConsignmentSystem.Application.DTOs.Vehicles;
using ConsignmentSystem.Domain.Entities;
using ConsignmentSystem.Domain.Repositories;
using Mapster;
using MediatR;

namespace ConsignmentSystem.Application.Features.Vehicles.Commands.AddVehicle
{
    public class AddVehicleCommandHandler : IRequestHandler<AddVehicleCommand, VehicleResponseDto>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IApplicationDbContext _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public AddVehicleCommandHandler(
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

        public async Task<VehicleResponseDto> Handle(AddVehicleCommand request, CancellationToken cancellationToken)
        {
            var vendor = await _vendorRepository.GetByIdAsync(request.VendorId, cancellationToken);
            if (vendor == null)
            {
                throw new NotFoundException(nameof(Vendor), request.VendorId);
            }

            if (await _vehicleRepository.ExistsByLicensePlateAsync(request.LicensePlate, cancellationToken))
            {
                throw new InvalidOperationException($"A vehicle with license plate '{request.LicensePlate}' is already registered.");
            }

            string currentUser = _currentUserService.Email ?? "Unknown";

            var vehicle = new Vehicle(request.VendorId, request.LicensePlate, request.DriverName, currentUser);

            await _vehicleRepository.AddAsync(vehicle, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return vehicle.Adapt<VehicleResponseDto>();
        }
    }
}