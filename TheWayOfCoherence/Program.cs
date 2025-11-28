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
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCookiePolicy();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Initialize roles and admin user
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
    var adminEmail = "Admin1";
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

app.Run();
