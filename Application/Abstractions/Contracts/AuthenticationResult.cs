using System;

namespace Application.Abstractions.Contracts
{
    public sealed record AuthenticationResult(bool Succeeded, bool IsAdmin, string? ErrorMessage)
    {
        public static AuthenticationResult Success(bool isAdmin) => new(true, isAdmin, null);

        public static AuthenticationResult Failure(string error) => new(false, false, error);
    }
}
