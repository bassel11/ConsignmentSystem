using ConsignmentSystem.Application.DTOs.Vehicles;
using MediatR;
using System.Text.Json.Serialization;

namespace ConsignmentSystem.Application.Features.Vehicles.Commands.AddVehicle
{
    public record AddVehicleCommand(string LicensePlate, string DriverName) : IRequest<VehicleResponseDto>
    {
        [JsonIgnore]
        public Guid VendorId { get; set; }
    }
}
