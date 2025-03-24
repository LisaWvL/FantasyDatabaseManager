using FantasyDB.Models;
using FantasyDB.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace FantasyDBStartup
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // 👇 Manual seed trigger (run only with "seed" argument)
            if (args.Length == 1 && args[0].ToLower() == "seed")
            {
                Console.WriteLine("🌱 Running JSON seed operation...");

                var factory = new AppDbContextFactory();
                using var context = factory.CreateDbContext(Array.Empty<string>());

                var seeder = new JsonSeeder(context);
                seeder.Seed();

                Console.WriteLine("✅ Done. Press any key to exit...");
                Console.ReadKey(); // 👈 Keeps the console open
                return;
            }

            // 🔧 Normal app startup
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;

            // Add services
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllersWithViews();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddRazorPages();
            builder.Services.AddScoped<IDropdownService, DropdownService>();
            builder.Logging.SetMinimumLevel(LogLevel.Debug);

            // Swagger for Development
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "FantasyDB API",
                        Version = "v1",
                        Description = "API documentation for FantasyDB"
                    });
                });
            }

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "FantasyDB API v1");
                    options.RoutePrefix = "swagger";
                });
            }

            app.MapControllers();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}
