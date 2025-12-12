using Application.Abstractions.Contracts;
using Application.Features.Auth.Commands;
using MediatR;

namespace Application.Features.Auth.Handlers
{
    public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IAuthService _auth;

        public LogoutCommandHandler(IAuthService auth)
        {
            _auth = auth;
        }

        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await _auth.LogoutAsync(cancellationToken);
        }
    }
}
