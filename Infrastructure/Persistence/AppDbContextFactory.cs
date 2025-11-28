using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Infrastructure.Persistence
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Find the TheWayOfCoherence project directory
            // When running from Infrastructure project, we need to go up one level
            var currentDir = Directory.GetCurrentDirectory();
            var basePath = Path.GetFullPath(Path.Combine(currentDir, "..", "TheWayOfCoherence"));
            
            // If that doesn't work, try going up two levels (from bin/Debug/net9.0)
            if (!Directory.Exists(basePath) || !File.Exists(Path.Combine(basePath, "appsettings.json")))
            {
                basePath = Path.GetFullPath(Path.Combine(currentDir, "..", "..", "..", "TheWayOfCoherence"));
            }
            
            // If still not found, try from solution root
            if (!Directory.Exists(basePath) || !File.Exists(Path.Combine(basePath, "appsettings.json")))
            {
                basePath = Path.GetFullPath(Path.Combine(currentDir, "..", "..", "TheWayOfCoherence"));
            }

            // Verify the path exists and has appsettings.json
            if (!Directory.Exists(basePath))
            {
                throw new DirectoryNotFoundException($"Could not find TheWayOfCoherence directory. Searched from: {currentDir}");
            }

            var appsettingsPath = Path.Combine(basePath, "appsettings.json");
            if (!File.Exists(appsettingsPath))
            {
                throw new FileNotFoundException($"Could not find appsettings.json at: {appsettingsPath}");
            }

            // Build configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"Connection string 'DefaultConnection' not found in appsettings.json at: {appsettingsPath}");
            }
            
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}

