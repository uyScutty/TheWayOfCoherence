using Application.Abstractions.Contracts;
using Application.Abstractions.Contracts.Gateways;
using Application.Features.Contact.Interfaces;
using Application.Features.Membership.Handlers;
using Application.Features.Membership.Interfaces;
using Application.Features.UserProfiles.Interfaces;

using Infrastructure.Events;
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
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
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


        return services;
    }
}

