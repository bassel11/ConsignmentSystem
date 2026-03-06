using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Application.Features.Auth.Commands.Register
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(v => v.FullName).NotEmpty().MaximumLength(100);

            RuleFor(v => v.Email)
                .NotEmpty()
                .EmailAddress().WithMessage("A valid email is required.");

            RuleFor(v => v.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(v => v.Role).NotEmpty();
        }
    }
}
