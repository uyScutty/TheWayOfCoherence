using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Identity;
using System.Security.Claims;

namespace TheWayOfCoherence.Services
{
    public class ServerAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public ServerAuthenticationStateProvider(
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            
            if (httpContext?.User?.Identity?.IsAuthenticated == true)
            {
                // Identity har allerede sat claims korrekt via cookie authentication
                // Brug HttpContext.User direkte for at undgå DbContext threading issues
                // Claims er allerede sat korrekt af Identity middleware
                return new AuthenticationState(httpContext.User);
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        // Public method to notify about authentication state changes
        public void NotifyAuthenticationStateChanged()
        {
            var authState = GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(authState);
        }
    }
}

