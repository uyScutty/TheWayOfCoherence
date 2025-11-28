using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TheWayOfCoherence.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public string Email { get; set; } = "";

        [BindProperty]
        public string Password { get; set; } = "";

        public string ErrorMessage { get; set; } = "";
        public bool IsLoggingIn { get; set; } = false;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Email og password skal udfyldes.";
                return Page();
            }

            IsLoggingIn = true;

            var user = await _userManager.FindByEmailAsync(Email);
            
            if (user == null)
            {
                ErrorMessage = "Ugyldig email eller password.";
                IsLoggingIn = false;
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(user, Password, true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Tjek om brugeren er admin og redirect til dashboard, ellers til members
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                return Redirect(isAdmin ? "/admin/dashboard" : "/members");
            }
            else if (result.IsLockedOut)
            {
                ErrorMessage = "Din konto er låst. Prøv igen senere.";
            }
            else if (result.IsNotAllowed)
            {
                ErrorMessage = "Du har ikke tilladelse til at logge ind.";
            }
            else
            {
                ErrorMessage = "Ugyldig email eller password.";
            }

            IsLoggingIn = false;
            return Page();
        }
    }
}

