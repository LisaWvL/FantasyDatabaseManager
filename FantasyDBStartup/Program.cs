using FantasyDB.Models;
using FantasyDB.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


// ✅ Load Configuration
var configuration = builder.Configuration;

// ✅ Add Database Context to Dependency Injection (DI)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// ✅ Add Controllers and Views (Supports MVC + Razor Pages)
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IDropdownService, DropdownService>();
builder.Logging.SetMinimumLevel(LogLevel.Debug);


// ✅ Add Swagger for API Documentation
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FantasyDB API",
        Version = "v1",
        Description = "API documentation for FantasyDB",
    });
});

// Force-load the FantasyDB assembly
var assembly = Assembly.Load("FantasyDB");
Console.WriteLine($"Loaded Assembly: {assembly.FullName}");

// ✅ Register Application Services
// ✅ Build Application
var app = builder.Build();

// ✅ Apply Pending Migrations (Ensures Database is Updated)
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    if (dbContext.Database.GetPendingMigrations().Any())
//    {
//        dbContext.Database.Migrate();
//    }
//}

// ✅ Configure Middleware Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
if (!Directory.Exists(app.Environment.WebRootPath))
{
    Directory.CreateDirectory(app.Environment.WebRootPath);
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// ✅ Enable Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "FantasyDB API v1");
        options.RoutePrefix = "swagger"; // Available at `/swagger`
    });
}
    // ✅ Set Up Routing with Generic Entity Catch-All
    app.MapControllerRoute(
        name: "entity-dynamic",
        pattern: "{entity}",
        defaults: new { controller = "GenericEntity", action = "Index" }
    );

    // ✅ Generic Edit and Create routes (e.g., /Character/Edit/5 or /Character/Create)
    app.MapControllerRoute(
        name: "entity-edit",
        pattern: "{entity}/{action}/{id?}",
        defaults: new { controller = "GenericEntity" }
    );

    // ✅ Fallback to Razor and MVC views for Home, About, etc.
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// ✅ Run Application
app.Run();
