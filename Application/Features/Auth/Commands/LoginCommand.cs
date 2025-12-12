using Application.Abstractions.Contracts;
using MediatR;

namespace Application.Features.Auth.Commands
{
    public sealed record LoginCommand(string Email, string Password, bool RememberMe = true)
        : IRequest<AuthenticationResult>;
}
