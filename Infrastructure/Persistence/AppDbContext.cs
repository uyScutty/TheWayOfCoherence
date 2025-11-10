using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Domain.Contact;
using Microsoft.EntityFrameworkCore;
using Domain.Content.Entities;
using Application.Features.Posts.Contracts;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

        public DbSet<BasePost> Posts => Set<BasePost>();
        public DbSet<Domain.Content.Entities.Post> Posts => Set<Domain.Content.Entities.Post>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Automatisk hent alle konfigurationer i assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
