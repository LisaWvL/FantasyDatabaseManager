using FantasyDB.Models;
using FantasyDB.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Load Configuration
var configuration = builder.Configuration;

// Add Database Context to Dependency Injection (DI)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// Add Controllers and Views and AutoMapper
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add Razor Pages
builder.Services.AddRazorPages();

// Add Dropdown Service
builder.Services.AddScoped<IDropdownService, DropdownService>();

// Configure Logging
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// Add Swagger for API Documentation (Development only)
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "FantasyDB API",
            Version = "v1",
            Description = "API documentation for FantasyDB",
        });
    });
}

var app = builder.Build();

// Configure Middleware Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Enable Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "FantasyDB API v1");
        options.RoutePrefix = "swagger";
    });
}

// Enable Attribute Routing for Controllers
app.MapControllers();

// Fallback to Razor and MVC views for Home, About, etc.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();