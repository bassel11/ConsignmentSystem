using ConsignmentSystem.Application.Common.Interfaces;
using MediatR;

namespace ConsignmentSystem.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IIdentityService _identityService;

        public LoginCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var (success, token, errorMessage) = await _identityService.LoginAsync(request.Email, request.Password);

            if (!success)
            {
                throw new InvalidOperationException(errorMessage);
            }

            return token;
        }
    }
}
