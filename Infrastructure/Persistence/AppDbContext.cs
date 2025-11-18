using Domain.Contact;
using Domain.Content.Entities;
using Domain.Membership;
using Domain.Users;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }


        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Post> Posts { get; set; } 

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<MembershipUser> Memberships { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relation mellem ApplicationUser og UserProfile
            builder.Entity<UserProfile>()
                .HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<UserProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
