using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TheWayOfCoherence.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("login")]
        [IgnoreAntiforgeryToken] // Tillad cross-origin requests fra Blazor
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { error = "Email og password skal udfyldes." });
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            
            if (user == null)
            {
                return Unauthorized(new { error = "Ugyldig email eller password." });
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Tjek brugerens rolle
                var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
                
                return Ok(new { 
                    success = true, 
                    redirectUrl = isAdmin ? "/owner" : "/patient" 
                });
            }
            else if (result.IsLockedOut)
            {
                return Unauthorized(new { error = "Din konto er låst. Prøv igen senere." });
            }
            else if (result.IsNotAllowed)
            {
                return Unauthorized(new { error = "Du har ikke tilladelse til at logge ind." });
            }
            else
            {
                return Unauthorized(new { error = "Ugyldig email eller password." });
            }
        }

        public class LoginRequest
        {
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
        }
    }
}

