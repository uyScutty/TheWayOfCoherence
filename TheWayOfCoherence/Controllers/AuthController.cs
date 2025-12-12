using Application.Features.Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TheWayOfCoherence.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [IgnoreAntiforgeryToken] // Tillad cross-origin requests fra Blazor
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _mediator.Send(new LoginCommand(
                request.Email,
                request.Password,
                request.RememberMe));

            if (result.Succeeded)
            {
                return Ok(new
                {
                    success = true,
                    redirectUrl = result.IsAdmin ? "/admin/dashboard" : "/members"
                });
            }

            return Unauthorized(new { error = result.ErrorMessage ?? "Ugyldig email eller password." });
        }

        [HttpPost("logout")]
        [HttpGet("logout")] // Tillad både GET og POST
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _mediator.Send(new LogoutCommand());
            return Redirect("/");
        }

        public class LoginRequest
        {
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
            public bool RememberMe { get; set; } = true;
        }
    }
}

