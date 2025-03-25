using FantasyDB;
using FantasyDB.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var projectDirectory = @"F:\FantasyDatabase\FantasyDatabaseManager\FantasyDB.Seeder";

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.SetBasePath(projectDirectory);
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));

        services.AddTransient<JsonSeeder>();
        services.AddTransient<SeedFks>();
        services.AddTransient<DatabaseResetService>();
    })
    .Build();

using var scope = builder.Services.CreateScope();
var resetService = scope.ServiceProvider.GetRequiredService<DatabaseResetService>();
resetService.RunInteractive();
