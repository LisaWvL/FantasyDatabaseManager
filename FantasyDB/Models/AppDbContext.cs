using System.Linq.Expressions;
using System.Linq;
using System;
using FantasyDB.Models;
using Microsoft.EntityFrameworkCore;
using static FantasyDB.Models.JunctionClasses;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Item> Items { get; set; }
    public DbSet<Calendar> Dates { get; set; }
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
    public DbSet<LanguageLocation> LanguagesLocations { get; set; }
    public DbSet<PlotPoint> PlotPoints { get; set; }
    public DbSet<PlotPointRiver> PlotPointsRivers { get; set; }
    public DbSet<PlotPointRoute> PlotPointsRoutes { get; set; }
    public DbSet<ConversationTurn> ConversationTurns { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Act> Acts { get; set; }
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<Scene> Scenes { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ApplyGlobalDeleteBehavior(modelBuilder);

        ConfigureItem(modelBuilder);
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
        ConfigureConversationTurns(modelBuilder);

        ConfigureBook(modelBuilder);
        ConfigureAct(modelBuilder);
        ConfigureChapter(modelBuilder);
        ConfigurePlotPoint(modelBuilder);
        ConfigureScene(modelBuilder);
        

        ConfigureJoin<PlotPointRiver>(modelBuilder, x => new { x.PlotPointId, x.RiverId });
        ConfigureJoin<PlotPointRoute>(modelBuilder, x => new { x.PlotPointId, x.RouteId });

    }


    private static void ApplyGlobalDeleteBehavior(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    private static void ConfigureJoin<T>(ModelBuilder modelBuilder, Expression<Func<T, object?>> keyExpression) where T : class
    {
        modelBuilder.Entity<T>().HasKey(keyExpression);
    }

    private static void ConfigureBook(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Acts)
            .WithOne(a => a.Book)
            .HasForeignKey(a => a.BookId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureAct(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Act>()
            .HasOne(a => a.Book)
            .WithMany(b => b.Acts)
            .HasForeignKey(a => a.BookId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
    private static void ConfigureChapter(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chapter>()
            .HasOne(c => c.Act)
            .WithMany(a => a.Chapters)
            .HasForeignKey(c => c.ActId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Chapter>()
            .HasOne(c => c.POVCharacter)
            .WithMany()
            .HasForeignKey(c => c.POVCharacterId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Chapter>()
            .HasMany(c => c.Scenes)
            .WithOne(s => s.Chapter)
            .HasForeignKey(s => s.ChapterId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }


    private static void ConfigureScene(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Scene>()
            .HasOne(s => s.Chapter)
            .WithMany(c => c.Scenes)
            .HasForeignKey(s => s.ChapterId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }



    private static void ConfigureCharacter(ModelBuilder modelBuilder)
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

        // One Character is POV in many Chapters referencing it by ChapterId
        modelBuilder.Entity<Character>()
            .HasOne(c => c.Chapter)
            .WithMany() // 🔁 mirror side
            .HasForeignKey(c => c.ChapterId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureConversationTurns(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConversationTurn>()
            .HasOne(c => c.PlotPoint)
            .WithMany()
            .HasForeignKey("PlotPointId");
    }

    private static void ConfigureItem(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>()
            .HasOne(a => a.Owner)
            .WithMany()
            .HasForeignKey("OwnerId");

        modelBuilder.Entity<Item>()
            .HasOne(a => a.Chapter)
            .WithMany()
            .HasForeignKey("ChapterId");
    }

    private static void ConfigureCalendar(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>()
            .HasOne(e => e.Date)
            .WithOne() // or .WithMany() if Calendar had multiple Events — but it doesn't need this.
            .HasForeignKey<Event>("CalendarId")
            .IsRequired(false);

    }


    private static void ConfigurePlotPoint(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlotPoint>()
            .HasOne(c => c.Chapter)
            .WithMany()
            .HasForeignKey("ChapterId");

        modelBuilder.Entity<PlotPoint>()
            .HasOne(c => c.StartDate)
            .WithMany()
            .HasForeignKey("StartDateId");

        modelBuilder.Entity<PlotPoint>()
            .HasOne(c => c.EndDate)
            .WithMany()
            .HasForeignKey("EndDateId");
    }


    private static void ConfigureCharacterRelationship(ModelBuilder modelBuilder)
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
            .HasOne(cr => cr.Chapter)
            .WithMany()
            .HasForeignKey("ChapterId");
    }


    private static void ConfigureEra(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Era>()
            .HasOne(e => e.Chapter)
            .WithMany()
            .HasForeignKey("ChapterId");
    }


    private static void ConfigureEvent(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>()
            .HasOne(e => e.Location)
            .WithMany(l => l.Events)
            .HasForeignKey(e => e.LocationId)
            .IsRequired(false); // Optional, since LocationId is nullable

        modelBuilder.Entity<Event>()
            .HasOne(e => e.Chapter)
            .WithMany()
            .HasForeignKey(e => e.ChapterId)
            .IsRequired(false);

        modelBuilder.Entity<Event>()
            .HasOne(e => e.Date)
            .WithMany()
            .HasForeignKey(e => e.CalendarId)
            .IsRequired(false);
    }




    private static void ConfigureLanguageLocation(ModelBuilder modelBuilder)
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


    private static void ConfigureFaction(ModelBuilder modelBuilder)
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
            .HasOne(f => f.Chapter)
            .WithMany()
            .HasForeignKey("ChapterId");
    }

    private static void ConfigureRiver(ModelBuilder modelBuilder)
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


    private static void ConfigureRoute(ModelBuilder modelBuilder)
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


    private static void ConfigurePriceExample(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PriceExample>()
            .Property(p => p.Price)
            .HasColumnType("int");
    }


}