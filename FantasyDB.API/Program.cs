using FantasyDB.Entities._Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FantasyDB.Features;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Dependency Injection
builder.Services.AddScoped<IDropdownService, DropdownService>();
//builder.Services.AddScoped<LoadAndRender>();
builder.Services.AddScoped<CardDesigner>();
builder.Services.AddScoped<DropHandlerService>();
builder.Services.AddScoped<UpdateFieldService>();
builder.Services.AddScoped<RelatedEntitiesService>();
// 🧠 Set schema path (adjust as needed)
var schemaJsonPath = Path.Combine(AppContext.BaseDirectory, "store/EntitySchemas.json");

// Inject with path manually
builder.Services.AddSingleton(new EntitySchemaProvider());

//builder.Services.AddSingleton(new EntitySchemaProvider(schemaJsonPath));

builder.Services.AddScoped<RelatedEntitiesService>();
builder.Services.AddScoped<AppDbContext>(); // Usually already registered

// Controllers with views from both assemblies
builder.Services.AddControllers()
    .AddApplicationPart(Assembly.GetExecutingAssembly())           // FantasyDB.API controllers
    .AddApplicationPart(typeof(FantasyDB.Features.CardRenderController).Assembly); // FantasyDB controllers

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:56507",  // creatorapp
                "http://localhost:8080"    // worldbuilderapp
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Swagger (only in dev)
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "FantasyDB API",
            Version = "v1"
        });
    });
}

var app = builder.Build();

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FantasyDB API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
