using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Application.Abstractions.Contracts.Gateways;
using Infrastructure.Gateways;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;
using Microsoft.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------
// 1️⃣ Core framework services
// ------------------------------------------------------
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddControllersWithViews();

// HttpClient for Blazor Server (til API kald)
// Konfigurer HttpClient til at inkludere cookies
builder.Services.AddHttpClient("WithCookies", client =>
{
    // Base address bliver sat i komponenten
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    UseCookies = true,
    CookieContainer = new System.Net.CookieContainer()
});

// Standard HttpClient (uden cookies)
builder.Services.AddHttpClient();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => false;
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

// ------------------------------------------------------
// 2️⃣ Application & Infrastructure layers
// ------------------------------------------------------
// Tilføj base URL til Python-microservice
string aiBaseUrl = builder.Configuration.GetValue<string>("AIService:BaseUrl")
                   ?? "http://localhost:8000"; // Python endpoint

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration, aiBaseUrl);

// ------------------------------------------------------
// 3️⃣ Authentication & Authorization (Identity)
// ------------------------------------------------------
// Identity er allerede konfigureret i AddInfrastructure med AddIdentity
// AddIdentity konfigurerer automatisk cookie authentication
// Vi skal bare tilføje authorization
builder.Services.AddAuthorization();

// Add HttpContextAccessor for authentication state provider
builder.Services.AddHttpContextAccessor();

// Add Blazor Server authentication state provider
// Bruger custom ServerAuthenticationStateProvider der læser fra HttpContext
builder.Services.AddScoped<Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider, 
    TheWayOfCoherence.Services.ServerAuthenticationStateProvider>();

// ------------------------------------------------------
// 4️⃣ Build the app
// ------------------------------------------------------
var app = builder.Build();

// ------------------------------------------------------
// 5️⃣ Middleware pipeline
// ------------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // HSTS (HTTP Strict Transport Security) - Force HTTPS
    app.UseHsts();
}

// Security Headers
app.Use(async (context, next) =>
{
    // Remove server header for security
    context.Response.Headers.Remove("Server");
    
    // Add security headers
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    
    // Content Security Policy (CSP) - adjust as needed for your app
    if (!app.Environment.IsDevelopment())
    {
        context.Response.Headers.Append("Content-Security-Policy", 
            "default-src 'self'; " +
            "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " +
            "style-src 'self' 'unsafe-inline'; " +
            "img-src 'self' data: https:; " +
            "font-src 'self' data:; " +
            "connect-src 'self' https:;");
    }
    
    await next();
});

// HTTPS Redirection - only in production
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

// Cookie Policy - Secure in production
app.UseCookiePolicy();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Initialize roles and admin user
// Wrap in try-catch to handle cases where migrations haven't run yet
try
{
    using (var scope = app.Services.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        
        // Create Admin role if it doesn't exist
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
        }
        
        // Create admin user if it doesn't exist
        var adminEmail = "Admin1@admin.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Administrator",
                CreatedAt = DateTime.UtcNow
            };
            
            var result = await userManager.CreateAsync(adminUser, "Admin123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
        else
        {
            // Ensure admin user is in Admin role
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
catch (Exception ex)
{
    // Log error but don't crash the app - migrations might not have run yet
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogWarning(ex, "Failed to initialize roles and admin user. This is normal if migrations haven't run yet.");
}

app.Run();
