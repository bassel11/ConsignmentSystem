using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Application.Features.Auth.Commands.Register
{
    public record RegisterUserCommand(
        string FullName,
        string Email,
        string Password,
        string Role) : IRequest<string>;
}
