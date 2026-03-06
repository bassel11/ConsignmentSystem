using ConsignmentSystem.Application.DTOs.Vendors;
using MediatR;

namespace ConsignmentSystem.Application.Features.Vendors.Commands.CreateVendor
{
    public record CreateVendorCommand(string Name, string ContactEmail, decimal DefaultCommissionPercentage)
        : IRequest<VendorResponseDto>;
}
