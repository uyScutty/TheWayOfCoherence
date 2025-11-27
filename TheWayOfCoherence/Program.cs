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
builder.Services.AddHttpClient();
builder.Services.AddScoped(sp => 
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient();
    httpClient.BaseAddress = new Uri(sp.GetRequiredService<NavigationManager>().BaseUri);
    return httpClient;
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
// 3️⃣ Authentication & Authorization (custom login)
// ------------------------------------------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Cookies";
    options.DefaultSignInScheme = "Cookies";
    options.DefaultChallengeScheme = "Cookies";
}).AddCookie("Cookies", options =>
{
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/access-denied";
});

builder.Services.AddAuthorization();

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

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Initialize roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    
    // Create Admin role if it doesn't exist
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
    }
}

app.Run();
