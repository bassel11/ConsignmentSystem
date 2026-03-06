using ConsignmentSystem.Application.DTOs.Consignments;
using ConsignmentSystem.Application.Features.Consignments.Commands.CreateReceipt.ConsignmentSystem.Application.Features.Consignments.Commands.CreateReceipt;
using FluentValidation;

namespace ConsignmentSystem.Application.Features.Consignments.Commands.CreateReceipt
{
    public class ConsignmentItemRequestDtoValidator : AbstractValidator<ConsignmentItemRequestDto>
    {
        public ConsignmentItemRequestDtoValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty().MaximumLength(250);
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
            RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0).WithMessage("Unit price cannot be negative.");
        }
    }
    public class CreateConsignmentReceiptCommandValidator : AbstractValidator<CreateConsignmentReceiptCommand>
    {
        public CreateConsignmentReceiptCommandValidator()
        {
            RuleFor(x => x.ReceiptNumber).NotEmpty().MaximumLength(50);

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one item must be included in the receipt.");
            RuleForEach(x => x.Items).SetValidator(new ConsignmentItemRequestDtoValidator());
        }
    }
}
