using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions.Contracts
{
    public interface IAuthService
    {
        Task<AuthenticationResult> LoginAsync(string email, string password, bool rememberMe, CancellationToken ct);

        Task LogoutAsync(CancellationToken ct);
    }
}
