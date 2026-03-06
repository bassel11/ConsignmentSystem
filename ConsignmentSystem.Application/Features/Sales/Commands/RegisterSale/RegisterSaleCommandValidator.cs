using FluentValidation;

namespace ConsignmentSystem.Application.Features.Sales.Commands.RegisterSale
{
    public class RegisterSaleCommandValidator : AbstractValidator<RegisterSaleCommand>
    {
        public RegisterSaleCommandValidator()
        {
            RuleFor(x => x.ConsignmentItemId).NotEmpty().WithMessage("Consignment Item ID is required.");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Sale quantity must be at least 1.");
            RuleFor(x => x.SalePrice).GreaterThanOrEqualTo(0).WithMessage("Sale price cannot be negative.");
        }
    }
}
