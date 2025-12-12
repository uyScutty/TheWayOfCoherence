using Application.Abstractions.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password, bool rememberMe, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return AuthenticationResult.Failure("Email og password skal udfyldes.");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return AuthenticationResult.Failure("Ugyldig email eller password.");
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                return AuthenticationResult.Success(isAdmin);
            }

            if (result.IsLockedOut)
            {
                return AuthenticationResult.Failure("Din konto er låst. Prøv igen senere.");
            }

            if (result.IsNotAllowed)
            {
                return AuthenticationResult.Failure("Du har ikke tilladelse til at logge ind.");
            }

            return AuthenticationResult.Failure("Ugyldig email eller password.");
        }

        public async Task LogoutAsync(CancellationToken ct)
        {
            await _signInManager.SignOutAsync();
        }
    }
}
