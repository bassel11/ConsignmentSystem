using ConsignmentSystem.Application.Common.Interfaces;
using MediatR;

namespace ConsignmentSystem.Application.Features.Auth.Commands.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IIdentityService _identityService;

        public RegisterUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var (success, errorMessage) = await _identityService.RegisterUserAsync(
                request.FullName,
                request.Email,
                request.Password,
                request.Role);

            if (!success)
            {
                throw new InvalidOperationException(errorMessage);
            }

            return $"User {request.Email} registered successfully as {request.Role}.";
        }
    }
}
