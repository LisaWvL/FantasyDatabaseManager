using System.Linq.Expressions;
using System.Linq;
using System;
using FantasyDB.Models;
using Microsoft.EntityFrameworkCore;
using static FantasyDB.Models.JunctionClasses;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Artifact> Artifacts { get; set; }
    public DbSet<Calendar> Calendar { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<CharacterRelationship> CharacterRelationships { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Era> Eras { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Faction> Factions { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<PriceExample> PriceExamples { get; set; }
    public DbSet<River> Rivers { get; set; }
    public DbSet<Route> Routes { get; set; }
    public DbSet<Snapshot> Snapshots { get; set; }
    public DbSet<JunctionClasses.SnapshotCharacter> SnapshotsCharacters { get; set; }
    public DbSet<JunctionClasses.SnapshotArtifact> SnapshotsArtifacts { get; set; }
    public DbSet<JunctionClasses.SnapshotEra> SnapshotsEras { get; set; }
    public DbSet<JunctionClasses.SnapshotEvent> SnapshotsEvents { get; set; }
    public DbSet<JunctionClasses.SnapshotFaction> SnapshotsFactions { get; set; }
    public DbSet<JunctionClasses.SnapshotLocation> SnapshotsLocations { get; set; }
    public DbSet<JunctionClasses.SnapshotCharacterRelationship> SnapshotsCharacterRelationships { get; set; }
    public DbSet<LanguageLocation> LanguagesLocations { get; set; }
    public DbSet<PlotPoint> PlotPoints { get; set; }
    public DbSet<PlotPointCharacter> PlotPointsCharacters { get; set; }
    public DbSet<PlotPointEvent> PlotPointsEvents { get; set; }
    public DbSet<PlotPointLocation> PlotPointsLocations { get; set; }
    public DbSet<PlotPointArtifact> PlotPointsArtifacts { get; set; }
    public DbSet<PlotPointFaction> PlotPointsFactions { get; set; }
    public DbSet<PlotPointEra> PlotPointsEras { get; set; }
    public DbSet<PlotPointCharacterRelationship> PlotPointsCharacterRelationships { get; set; }
    public DbSet<PlotPointRiver> PlotPointsRivers { get; set; }
    public DbSet<PlotPointRoute> PlotPointsRoutes { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ApplyGlobalDeleteBehavior(modelBuilder);

        ConfigureArtifact(modelBuilder);
        ConfigureCalendar(modelBuilder);
        ConfigureCharacter(modelBuilder);
        ConfigureCharacterRelationship(modelBuilder);
        ConfigureEra(modelBuilder);
        ConfigureEvent(modelBuilder);
        ConfigureFaction(modelBuilder);
        ConfigureRiver(modelBuilder);
        ConfigureRoute(modelBuilder);
        ConfigurePriceExample(modelBuilder);
        ConfigureLanguageLocation(modelBuilder);


        ConfigureJoin<SnapshotCharacter>(modelBuilder, x => new { x.SnapshotId, x.CharacterId });
        ConfigureJoin<SnapshotArtifact>(modelBuilder, x => new { x.SnapshotId, x.ArtifactId });
        ConfigureJoin<SnapshotEra>(modelBuilder, x => new { x.SnapshotId, x.EraId });
        ConfigureJoin<SnapshotEvent>(modelBuilder, x => new { x.SnapshotId, x.EventId });
        ConfigureJoin<SnapshotFaction>(modelBuilder, x => new { x.SnapshotId, x.FactionId });
        ConfigureJoin<SnapshotLocation>(modelBuilder, x => new { x.SnapshotId, x.LocationId });
        ConfigureJoin<SnapshotCharacterRelationship>(modelBuilder, x => new { x.SnapshotId, x.CharacterRelationshipId });
        ConfigureJoin<PlotPointCharacter>(modelBuilder, x => new { x.PlotPointId, x.CharacterId });
        ConfigureJoin<PlotPointEvent>(modelBuilder, x => new { x.PlotPointId, x.EventId });
        ConfigureJoin<PlotPointLocation>(modelBuilder, x => new { x.PlotPointId, x.LocationId });
        ConfigureJoin<PlotPointArtifact>(modelBuilder, x => new { x.PlotPointId, x.ArtifactId });
        ConfigureJoin<PlotPointFaction>(modelBuilder, x => new { x.PlotPointId, x.FactionId });
        ConfigureJoin<PlotPointEra>(modelBuilder, x => new { x.PlotPointId, x.EraId });
        ConfigureJoin<PlotPointCharacterRelationship>(modelBuilder, x => new { x.PlotPointId, x.CharacterRelationshipId });
        ConfigureJoin<PlotPointRiver>(modelBuilder, x => new { x.PlotPointId, x.RiverId });
        ConfigureJoin<PlotPointRoute>(modelBuilder, x => new { x.PlotPointId, x.RouteId });

    }


    private void ApplyGlobalDeleteBehavior(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    private void ConfigureJoin<T>(ModelBuilder modelBuilder, Expression<Func<T, object>> keyExpression) where T : class
    {
        modelBuilder.Entity<T>().HasKey(keyExpression);
    }

   
    private void ConfigureArtifact(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Artifact>()
            .HasOne(a => a.Owner)
            .WithMany()
            .HasForeignKey("OwnerId");

        modelBuilder.Entity<Artifact>()
            .HasOne(a => a.Snapshot)
            .WithMany()
            .HasForeignKey("SnapshotId");
    }


    private void ConfigureCalendar(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Calendar>()
            .HasOne(c => c.Event)
            .WithMany()
            .HasForeignKey("EventId");
    }


    private void ConfigureCharacter(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Character>()
            .HasOne(c => c.Faction)
            .WithMany()
            .HasForeignKey("FactionId");

        modelBuilder.Entity<Character>()
            .HasOne(c => c.Location)
            .WithMany()
            .HasForeignKey("LocationId");

        modelBuilder.Entity<Character>()
            .HasOne(c => c.Language)
            .WithMany()
            .HasForeignKey("LanguageId");

        modelBuilder.Entity<Character>()
            .HasOne(c => c.Snapshot)
            .WithMany()
            .HasForeignKey("SnapshotId");
    }


    private void ConfigureCharacterRelationship(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CharacterRelationship>()
            .HasOne(cr => cr.Character1)
            .WithMany()
            .HasForeignKey("Character1Id");

        modelBuilder.Entity<CharacterRelationship>()
            .HasOne(cr => cr.Character2)
            .WithMany()
            .HasForeignKey("Character2Id");

        modelBuilder.Entity<CharacterRelationship>()
            .HasOne(cr => cr.Snapshot)
            .WithMany()
            .HasForeignKey("SnapshotId");
    }


    private void ConfigureEra(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Era>()
            .HasOne(e => e.Snapshot)
            .WithMany()
            .HasForeignKey("SnapshotId");
    }


    private void ConfigureEvent(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>()
            .HasOne(e => e.Location)
            .WithMany(l => l.Events)
            .HasForeignKey(e => e.LocationId)
            .OnDelete(DeleteBehavior.SetNull); // or .SetNull if you'd like auto-unlinking

        modelBuilder.Entity<Event>()
            .HasOne(e => e.Snapshot)
            .WithMany()
            .HasForeignKey(e => e.SnapshotId)
            .OnDelete(DeleteBehavior.SetNull);
    }


    private void ConfigureLanguageLocation(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LanguageLocation>()
            .HasKey(ll => new { ll.LanguageId, ll.LocationId });

        modelBuilder.Entity<LanguageLocation>()
            .HasOne(ll => ll.Language)
            .WithMany(l => l.LanguageLocations)
            .HasForeignKey(ll => ll.LanguageId)
            .OnDelete(DeleteBehavior.Restrict); // Avoids cascading delete loops

        modelBuilder.Entity<LanguageLocation>()
            .HasOne(ll => ll.Location)
            .WithMany(l => l.LanguageLocations)
            .HasForeignKey(ll => ll.LocationId)
            .OnDelete(DeleteBehavior.Restrict);
    }


    private void ConfigureFaction(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Faction>()
            .HasOne(f => f.Founder)
            .WithMany()
            .HasForeignKey("FounderId");

        modelBuilder.Entity<Faction>()
            .HasOne(f => f.Leader)
            .WithMany()
            .HasForeignKey("LeaderId");

        modelBuilder.Entity<Faction>()
            .HasOne(f => f.HQLocation)
            .WithMany()
            .HasForeignKey("HQLocationId");

        modelBuilder.Entity<Faction>()
            .HasOne(f => f.Snapshot)
            .WithMany()
            .HasForeignKey("SnapshotId");
    }

    private void ConfigureRiver(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<River>()
            .HasOne(r => r.SourceLocation)
            .WithMany()
            .HasForeignKey("SourceLocationId");

        modelBuilder.Entity<River>()
            .HasOne(r => r.DestinationLocation)
            .WithMany()
            .HasForeignKey("DestinationLocationId");
    }


    private void ConfigureRoute(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Route>()
            .HasOne(r => r.From)
            .WithMany()
            .HasForeignKey("FromId");

        modelBuilder.Entity<Route>()
            .HasOne(r => r.To)
            .WithMany()
            .HasForeignKey("ToId");
    }


    private void ConfigurePriceExample(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PriceExample>()
            .Property(p => p.Price)
            .HasColumnType("int");
    }


}