using Application.Abstractions.Contracts;
using Application.Features.Auth.Commands;
using MediatR;

namespace Application.Features.Auth.Handlers
{
    public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResult>
    {
        private readonly IAuthService _auth;

        public LoginCommandHandler(IAuthService auth)
        {
            _auth = auth;
        }

        public Task<AuthenticationResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return _auth.LoginAsync(request.Email, request.Password, request.RememberMe, cancellationToken);
        }
    }
}
