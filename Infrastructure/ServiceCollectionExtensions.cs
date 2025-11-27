using Application.Abstractions.Contracts;
using Application.Abstractions.Contracts.Gateways;
using Application.Features.Contact.Interfaces;
using Application.Features.Membership.Interfaces;
using Application.Features.Posts.Contracts;
using Application.Features.UserProfiles.Interfaces;
using Infrastructure.Gateways;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config, string aiBaseUrl)
    {
        // DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        // Identity
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders()
        .AddSignInManager<SignInManager<ApplicationUser>>();

        // Repositories, events og notifiers
        services.AddScoped<IContactMessageRepository, ContactMessageRepository>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        services.AddScoped<IEmailNotifier, EmailNotifierSmtp>();
        services.AddScoped<IMembershipRepository, MembershipRepository>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IUserLookupService, UserLookupService>();

        // Eksisterende AI Gateway
        services.AddHttpClient<IAIChatGateway, AIChatGateway>(client =>
        {
            client.BaseAddress = new Uri(aiBaseUrl); // eksisterende AI service
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        // Ny Python Chat Gateway
        services.AddHttpClient<IChatService, PythonChatGateway>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:8000"); // Python microservice URL
            client.Timeout = TimeSpan.FromMinutes(5);
        });

        return services;
    }
}
