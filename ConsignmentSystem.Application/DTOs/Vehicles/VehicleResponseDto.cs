using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Application.DTOs.Vehicles
{
    public record VehicleResponseDto(
        Guid Id,
        Guid VendorId,
        string LicensePlate,
        string DriverName,
        DateTime CreatedAt);
}
