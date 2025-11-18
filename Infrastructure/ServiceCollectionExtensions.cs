using Application.Abstractions.Contracts.Gateways;
using Application.Features.Contact.Interfaces;
using Application.Features.Membership.Interfaces;
using Application.Features.UserProfiles.Interfaces;

using Domain.Shared;

using Infrastructure.Events;
using Infrastructure.Gateways;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Events;
using Infrastructure.Persistence.Gateways;
using Infrastructure.Persistence.Repositories;

using Infrastructure.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config, string aiBaseUrl)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

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



        // ... dine repos / events / email
        services.AddScoped<IContactMessageRepository, ContactMessageRepository>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        services.AddScoped<IEmailNotifier, EmailNotifierSmtp>();
        services.AddScoped<IMembershipRepository, MembershipRepository>();
        services.AddScoped<IIdentityService, IdentityService>();

        services.AddScoped<IMembershipRepository, MembershipRepository>();


        //AI Gateway 
        services.AddHttpClient<IAIChatGateway, AIChatGateway>(client =>
        {
            client.BaseAddress = new Uri(aiBaseUrl); //Skal ændres til den hostede python microservice (til bots)
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        return services;
    }
}

