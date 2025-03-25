using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // 🔥 Fix: Correctly find the base solution directory
        string basePath = Directory.GetCurrentDirectory(); // Current working dir
        string startupConfigPath = Path.Combine(basePath, "appsettings.json");
        string dbConfigPath = Path.Combine(basePath, "..", "FantasyDB", "appsettings.json");

        Console.WriteLine($"🔍 Checking for appsettings.json in:");
        Console.WriteLine($" - FantasyDB: {startupConfigPath}");
        Console.WriteLine($" - FantasyDB: {dbConfigPath}");

        string configPath = File.Exists(startupConfigPath) ? startupConfigPath :
                            File.Exists(dbConfigPath) ? dbConfigPath :
                            throw new FileNotFoundException("⚠️ Could not find appsettings.json in either FantasyDB or FantasyDB!");

        Console.WriteLine($"✅ Using configuration file: {configPath}");

        var config = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(configPath)!) // 🔥 Correct path
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // ✅ Ensure migrations are placed in FantasyDB
        optionsBuilder.UseSqlServer(
            config.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly("FantasyDB")
        );

        Console.WriteLine("🚀 Successfully created AppDbContext for EF Core tooling.");
        return new AppDbContext(optionsBuilder.Options);
    }
}
