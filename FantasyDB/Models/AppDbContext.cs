using FantasyDB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Artifact> Artifact { get; set; }
    public DbSet<Calendar> Calendar { get; set; }
    public DbSet<Character> Character { get; set; }
    public DbSet<CharacterRelationship> CharacterRelationship { get; set; }
    public DbSet<Currency> Currency { get; set; }
    public DbSet<Era> Era { get; set; }
    public DbSet<Event> Event { get; set; }
    public DbSet<Faction> Faction { get; set; }
    public DbSet<Language> Language { get; set; }
    public DbSet<Location> Location { get; set; }
    public DbSet<PriceExample> PriceExample { get; set; }
    public DbSet<River> River { get; set; }
    public DbSet<Route> Route { get; set; }
    public DbSet<Snapshot> Snapshot { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>().Ignore(l => l.ChildLocationId);
        modelBuilder.Entity<Location>().Ignore(l => l.ParentLocationId);
        modelBuilder.Entity<Location>().Ignore(l => l.EventId);
        modelBuilder.Entity<Location>().Ignore(l => l.LanguageId);
        modelBuilder.Entity<Location>().Ignore(l => l.SnapshotId);
        modelBuilder.Entity<Faction>().Ignore(f => f.FounderId);
        modelBuilder.Entity<Faction>().Ignore(f => f.LeaderId);
        modelBuilder.Entity<Faction>().Ignore(f => f.HQLocationId);
        modelBuilder.Entity<Faction>().Ignore(f => f.SnapshotId);
        modelBuilder.Entity<Artifact>().Ignore(a => a.OwnerId);
        modelBuilder.Entity<Artifact>().Ignore(a => a.SnapshotId);
        modelBuilder.Entity<CharacterRelationship>().Ignore(cr => cr.Character1Id);
        modelBuilder.Entity<CharacterRelationship>().Ignore(cr => cr.Character2Id);
        modelBuilder.Entity<CharacterRelationship>().Ignore(cr => cr.SnapshotId);
        modelBuilder.Entity<Era>().Ignore(e => e.SnapshotId);
        modelBuilder.Entity<Event>().Ignore(e => e.LocationId);
        modelBuilder.Entity<Event>().Ignore(e => e.SnapshotId);
        modelBuilder.Entity<River>().Ignore(r => r.SourceLocationId);
        modelBuilder.Entity<River>().Ignore(r => r.DestinationLocationId);
        modelBuilder.Entity<Route>().Ignore(r => r.FromId);
        modelBuilder.Entity<Route>().Ignore(r => r.ToId);






        modelBuilder.Entity<PriceExample>()
        .Property(p => p.Price)
        .HasColumnType("decimal(18,2)");
        base.OnModelCreating(modelBuilder);
    }

private List<T> LoadSeedData<T>(string fileName)
    {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "FantasyDBStartup/SeedData/", fileName);
        if (!File.Exists(fullPath)) return new List<T>();

        var json = File.ReadAllText(fullPath);
        return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
    }
}
