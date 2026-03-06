using MediatR;
using System.Text.Json.Serialization;

namespace ConsignmentSystem.Application.Features.Vendors.Commands.UpdateVendor
{
    public record UpdateVendorCommand(
        string Name,
        string ContactEmail,
        decimal DefaultCommissionPercentage) : IRequest
    {
        [JsonIgnore]
        public Guid Id { get; set; }
    }
}
