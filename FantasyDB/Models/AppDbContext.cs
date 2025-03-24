using FantasyDB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using static FantasyDB.Models.JunctionClasses;

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
    public DbSet<JunctionClasses.SnapshotCharacter> SnapshotCharacter { get; set; }
    public DbSet<JunctionClasses.SnapshotArtifact> SnapshotArtifact { get; set; }
    public DbSet<JunctionClasses.SnapshotEra> SnapshotEra { get; set; }
    public DbSet<JunctionClasses.SnapshotEvent> SnapshotEvent { get; set; }
    public DbSet<JunctionClasses.SnapshotFaction> SnapshotFaction { get; set; }
    public DbSet<JunctionClasses.SnapshotLocation> SnapshotLocation { get; set; }
    public DbSet<JunctionClasses.SnapshotCharacterRelationship> SnapshotCharacterRelationship { get; set; }
    public DbSet<LocationLocation> LocationLocation { get; set; }
    public DbSet<LocationEvent> LocationEvent { get; set; }
    public DbSet<LanguageLocation> LanguageLocation { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Artifact
        modelBuilder.Entity<Artifact>()
            .HasOne(a => a.Owner)
            .WithMany()
            .HasForeignKey("OwnerId");

        modelBuilder.Entity<Artifact>()
            .HasOne(a => a.Snapshot)
            .WithMany()
            .HasForeignKey("SnapshotId");

        // Calendar
        modelBuilder.Entity<Calendar>();

        // Character
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

        // CharacterRelationship
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

        // Era
        modelBuilder.Entity<Era>()
            .HasOne(e => e.Snapshot)
            .WithMany()
            .HasForeignKey("SnapshotId");

        // Event
        modelBuilder.Entity<Event>()
            .HasOne(e => e.Location)
            .WithMany()
            .HasForeignKey("LocationId");

        modelBuilder.Entity<Event>()
            .HasOne(e => e.Snapshot)
            .WithMany()
            .HasForeignKey("SnapshotId");

        // Faction
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


        modelBuilder.Entity<Location>()
            .HasOne(l => l.ParentLocation)
            .WithMany()
            .HasForeignKey("ParentLocationId");

        modelBuilder.Entity<Location>()
            .HasOne(l => l.Language)
            .WithMany()
            .HasForeignKey("LanguageId");

        modelBuilder.Entity<Location>()
            .HasOne(l => l.Snapshot)
            .WithMany()
            .HasForeignKey("SnapshotId");

        // River
        modelBuilder.Entity<River>()
            .HasOne(r => r.SourceLocation)
            .WithMany()
            .HasForeignKey("SourceLocationId");

        modelBuilder.Entity<River>()
            .HasOne(r => r.DestinationLocation)
            .WithMany()
            .HasForeignKey("DestinationLocationId");

        // Route
        modelBuilder.Entity<Route>()
            .HasOne(r => r.From)
            .WithMany()
            .HasForeignKey("FromId");

        modelBuilder.Entity<Route>()
            .HasOne(r => r.To)
            .WithMany()
            .HasForeignKey("ToId");

        // PriceExample
        modelBuilder.Entity<PriceExample>()
            .Property(p => p.Price)
            .HasColumnType("int");

        modelBuilder.Entity<Location>()
          .HasOne(l => l.ParentLocation)
          .WithMany()
          .HasForeignKey(l => l.ParentLocationId);


        modelBuilder.Entity<JunctionClasses.SnapshotArtifact>()
            .HasKey(sa => new { sa.SnapshotId, sa.ArtifactId });

        modelBuilder.Entity<JunctionClasses.SnapshotArtifact>()
            .HasOne(sa => sa.Snapshot)
            .WithMany()
            .HasForeignKey(sa => sa.SnapshotId);

        modelBuilder.Entity<JunctionClasses.SnapshotArtifact>()
            .HasOne(sa => sa.Artifact)
            .WithMany()
            .HasForeignKey(sa => sa.ArtifactId);

        modelBuilder.Entity<JunctionClasses.SnapshotEra>()
            .HasKey(se => new { se.SnapshotId, se.EraId });

        modelBuilder.Entity<JunctionClasses.SnapshotEra>()
            .HasOne(se => se.Snapshot)
            .WithMany()
            .HasForeignKey(se => se.SnapshotId);

        modelBuilder.Entity<JunctionClasses.SnapshotEra>()
            .HasOne(se => se.Era)
            .WithMany()
            .HasForeignKey(se => se.EraId);

        modelBuilder.Entity<JunctionClasses.SnapshotEvent>()
            .HasKey(se => new { se.SnapshotId, se.EventId });

        modelBuilder.Entity<JunctionClasses.SnapshotEvent>()
            .HasOne(se => se.Snapshot)
            .WithMany()
            .HasForeignKey(se => se.SnapshotId);

        modelBuilder.Entity<JunctionClasses.SnapshotEvent>()
            .HasOne(se => se.Event)
            .WithMany()
            .HasForeignKey(se => se.EventId);

        modelBuilder.Entity<JunctionClasses.SnapshotFaction>()
            .HasKey(sf => new { sf.SnapshotId, sf.FactionId });

        modelBuilder.Entity<JunctionClasses.SnapshotFaction>()
            .HasOne(sf => sf.Snapshot)
            .WithMany()
            .HasForeignKey(sf => sf.SnapshotId);

        modelBuilder.Entity<JunctionClasses.SnapshotFaction>()
            .HasOne(sf => sf.Faction)
            .WithMany()
            .HasForeignKey(sf => sf.FactionId);

        modelBuilder.Entity<JunctionClasses.SnapshotLocation>()
            .HasKey(sl => new { sl.SnapshotId, sl.LocationId });

        modelBuilder.Entity<JunctionClasses.SnapshotLocation>()
            .HasOne(sl => sl.Snapshot)
            .WithMany()
            .HasForeignKey(sl => sl.SnapshotId);

        modelBuilder.Entity<JunctionClasses.SnapshotLocation>()
            .HasOne(sl => sl.Location)
            .WithMany()
            .HasForeignKey(sl => sl.LocationId);

        modelBuilder.Entity<JunctionClasses.SnapshotCharacterRelationship>()
            .HasKey(scr => new { scr.SnapshotId, scr.CharacterRelationshipId });

        modelBuilder.Entity<JunctionClasses.SnapshotCharacterRelationship>()
            .HasOne(scr => scr.Snapshot)
            .WithMany()
            .HasForeignKey(scr => scr.SnapshotId);

        modelBuilder.Entity<JunctionClasses.SnapshotCharacterRelationship>()
            .HasOne(scr => scr.CharacterRelationship)
            .WithMany()
            .HasForeignKey(scr => scr.CharacterRelationshipId);

        // Junction Table Configurations
        modelBuilder.Entity<SnapshotCharacter>()
            .HasKey(sc => new { sc.SnapshotId, sc.CharacterId });

        modelBuilder.Entity<SnapshotCharacter>()
            .HasOne(sc => sc.Snapshot)
            .WithMany()
            .HasForeignKey(sc => sc.SnapshotId);

        modelBuilder.Entity<SnapshotCharacter>()
            .HasOne(sc => sc.Character)
            .WithMany()
            .HasForeignKey(sc => sc.CharacterId);

        modelBuilder.Entity<SnapshotArtifact>()
            .HasKey(sa => new { sa.SnapshotId, sa.ArtifactId });

        modelBuilder.Entity<SnapshotArtifact>()
            .HasOne(sa => sa.Snapshot)
            .WithMany()
            .HasForeignKey(sa => sa.SnapshotId);

        modelBuilder.Entity<SnapshotArtifact>()
            .HasOne(sa => sa.Artifact)
            .WithMany()
            .HasForeignKey(sa => sa.ArtifactId);

        modelBuilder.Entity<SnapshotEra>()
            .HasKey(se => new { se.SnapshotId, se.EraId });

        modelBuilder.Entity<SnapshotEra>()
            .HasOne(se => se.Snapshot)
            .WithMany()
            .HasForeignKey(se => se.SnapshotId);

        modelBuilder.Entity<SnapshotEra>()
            .HasOne(se => se.Era)
            .WithMany()
            .HasForeignKey(se => se.EraId);

        modelBuilder.Entity<SnapshotEvent>()
            .HasKey(se => new { se.SnapshotId, se.EventId });

        modelBuilder.Entity<SnapshotEvent>()
            .HasOne(se => se.Snapshot)
            .WithMany()
            .HasForeignKey(se => se.SnapshotId);

        modelBuilder.Entity<SnapshotEvent>()
            .HasOne(se => se.Event)
            .WithMany()
            .HasForeignKey(se => se.EventId);

        modelBuilder.Entity<SnapshotFaction>()
            .HasKey(sf => new { sf.SnapshotId, sf.FactionId });

        modelBuilder.Entity<SnapshotFaction>()
            .HasOne(sf => sf.Snapshot)
            .WithMany()
            .HasForeignKey(sf => sf.SnapshotId);

        modelBuilder.Entity<SnapshotFaction>()
            .HasOne(sf => sf.Faction)
            .WithMany()
            .HasForeignKey(sf => sf.FactionId);

        modelBuilder.Entity<SnapshotLocation>()
            .HasKey(sl => new { sl.SnapshotId, sl.LocationId });

        modelBuilder.Entity<SnapshotLocation>()
            .HasOne(sl => sl.Snapshot)
            .WithMany()
            .HasForeignKey(sl => sl.SnapshotId);

        modelBuilder.Entity<SnapshotLocation>()
            .HasOne(sl => sl.Location)
            .WithMany()
            .HasForeignKey(sl => sl.LocationId);

        modelBuilder.Entity<SnapshotCharacterRelationship>()
            .HasKey(scr => new { scr.SnapshotId, scr.CharacterRelationshipId });

        modelBuilder.Entity<SnapshotCharacterRelationship>()
            .HasOne(scr => scr.Snapshot)
            .WithMany()
            .HasForeignKey(scr => scr.SnapshotId);

        modelBuilder.Entity<SnapshotCharacterRelationship>()
            .HasOne(scr => scr.CharacterRelationship)
            .WithMany()
            .HasForeignKey(scr => scr.CharacterRelationshipId);

        // Location - Location Junction Table
        modelBuilder.Entity<LocationLocation>()
            .HasKey(ll => new { ll.LocationId, ll.ChildLocationId });

        modelBuilder.Entity<LocationLocation>()
            .HasOne(ll => ll.Location)
            .WithMany()
            .HasForeignKey(ll => ll.LocationId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<LocationLocation>()
            .HasOne(ll => ll.ChildLocation)
            .WithMany()
            .HasForeignKey(ll => ll.ChildLocationId)
            .OnDelete(DeleteBehavior.NoAction);

        // Location - Event Junction Table
        modelBuilder.Entity<LocationEvent>()
            .HasKey(le => new { le.LocationId, le.EventId });

        modelBuilder.Entity<LocationEvent>()
            .HasOne(le => le.Location)
            .WithMany()
            .HasForeignKey(le => le.LocationId);

        modelBuilder.Entity<LocationEvent>()
            .HasOne(le => le.Event)
            .WithMany()
            .HasForeignKey(le => le.EventId);

        // Location - Language Junction Table
        modelBuilder.Entity<LanguageLocation>()
            .HasKey(ll => new { ll.LocationId, ll.LanguageId });

        modelBuilder.Entity<LanguageLocation>()
            .HasOne(ll => ll.Location)
            .WithMany()
            .HasForeignKey(ll => ll.LocationId);

        modelBuilder.Entity<LanguageLocation>()
            .HasOne(ll => ll.Language)
            .WithMany()
            .HasForeignKey(ll => ll.LanguageId);

        base.OnModelCreating(modelBuilder);
    }
}