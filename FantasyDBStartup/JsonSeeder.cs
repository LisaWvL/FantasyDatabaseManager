using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using FantasyDB.Models;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Utilities; // ✅ Import the shared JSON converter


public class JsonSeeder
{
    private readonly AppDbContext _context;

    public JsonSeeder(AppDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        Console.WriteLine("🌱 Seeding database...");

        SeedTable<Snapshot>("Snapshots.json", _context.Snapshot);
        SeedTable<Location>("locations.json", _context.Location);
        SeedTable<Character>("characters.json", _context.Character);
        SeedTable<CharacterRelationship>("characterRelationships.json", _context.CharacterRelationship);
        SeedTable<Artifact>("artifacts.json", _context.Artifact);
        SeedTable<River>("rivers.json", _context.River);
        SeedTable<FantasyDB.Models.Route>("routes.json", _context.Route);
        SeedTable<Faction>("factions.json", _context.Faction);
        SeedTable<Event>("events.json", _context.Event);
        SeedTable<Currency>("currency.json", _context.Currency);
        SeedTable<Calendar>("calendar.json", _context.Calendar);
        SeedTable<PriceExample>("priceExamples.json", _context.PriceExample);
        SeedTable<Era>("eras.json", _context.Era);

        _context.SaveChanges();
        Console.WriteLine("✅ Seeding complete!");
    }

    private void SeedTable<T>(string fileName, DbSet<T> dbSet) where T : class
    {
        string fullPath = Path.Combine("F:\\FantasyDatabase\\FantasyDatabaseManager\\FantasyDBStartup\\Seed Data\\", fileName);
        Console.WriteLine($"🔍 [DEBUG] Looking for {fileName} at: {fullPath}");

        if (!File.Exists(fullPath))
        {
            Console.WriteLine($"❌ [ERROR] File not found at {fullPath}");
            return;
        }

        var jsonData = File.ReadAllText(fullPath);
        Console.WriteLine($"📄 [DEBUG] Successfully read {fileName} from {fullPath}");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new StringOrArrayConverter() } // ✅ Use the fixed converter
        };

        try
        {
            var data = JsonSerializer.Deserialize<List<T>>(jsonData, options);

            if (data == null || !data.Any())
            {
                Console.WriteLine($"⚠️ [WARNING] No data found in {fileName}, skipping...");
                return;
            }

            if (!dbSet.Any()) // Prevents duplicate inserts
            {
                dbSet.AddRange(data);
                Console.WriteLine($"✅ Inserted {data.Count} records into {typeof(T).Name}.");
            }
            else
            {
                Console.WriteLine($"ℹ️ {typeof(T).Name} already has data, skipping...");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ [ERROR] Failed to process {fileName}: {ex.Message}");
        }
    }

}

