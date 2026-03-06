using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Application.Features.Vendors.Commands.CreateVendor
{
    public class CreateVendorCommandValidator : AbstractValidator<CreateVendorCommand>
    {
        public CreateVendorCommandValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Vendor name is required.")
                .MaximumLength(200).WithMessage("Vendor name must not exceed 200 characters.");

            RuleFor(v => v.ContactEmail)
                .NotEmpty().WithMessage("Contact email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MaximumLength(150).WithMessage("Email must not exceed 150 characters.");
        }
    }
}
