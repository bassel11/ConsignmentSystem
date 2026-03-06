using FluentValidation;

namespace ConsignmentSystem.Application.Features.Vehicles.Commands.AddVehicle
{
    public class AddVehicleCommandValidator : AbstractValidator<AddVehicleCommand>
    {
        public AddVehicleCommandValidator()
        {
            RuleFor(v => v.LicensePlate)
                .NotEmpty().WithMessage("License plate is required.")
                .MaximumLength(50).WithMessage("License plate must not exceed 50 characters.");

            RuleFor(v => v.DriverName)
                .NotEmpty().WithMessage("Driver name is required.")
                .MaximumLength(200).WithMessage("Driver name must not exceed 200 characters.");
        }
    }
}
