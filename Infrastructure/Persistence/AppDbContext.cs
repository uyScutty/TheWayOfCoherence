using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Domain.Contact;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Automatisk hent alle konfigurationer i assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }

