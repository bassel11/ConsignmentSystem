using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsignmentSystem.Application.Features.Auth.Commands.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<string>;
}
