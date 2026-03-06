using FluentValidation;

namespace ConsignmentSystem.Application.Features.Invoices.Commands.GenerateInvoice
{
    public class GenerateInvoiceCommandValidator : AbstractValidator<GenerateInvoiceCommand>
    {
        public GenerateInvoiceCommandValidator()
        {
            RuleFor(x => x.InvoiceNumber).NotEmpty().MaximumLength(50);

            RuleFor(x => x.OverrideCommissionAmount).GreaterThanOrEqualTo(0).WithMessage("Commission cannot be negative.");
            RuleFor(x => x.ExpensesAmount).GreaterThanOrEqualTo(0).WithMessage("Expenses cannot be negative.");
        }
    }
}
