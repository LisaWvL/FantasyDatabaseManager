using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using FantasyDB.Models;
using Microsoft.EntityFrameworkCore;


namespace FantasyDBStartup
{
    public class JsonSeeder
    {
        private readonly AppDbContext _context;
        public JsonSeeder(AppDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            Console.WriteLine("🌱 Starting database seeding...");

            using var transaction = _context.Database.BeginTransaction();
            try
            {

                SeedTable<Snapshot>("Snapshots.json", _context.Snapshot);
                SeedTable<Location>("Locations.json", _context.Location);
                SeedTable<Character>("Characters.json", _context.Character);
                SeedTable<Artifact>("Artifacts.json", _context.Artifact);
                SeedTable<River>("Rivers.json", _context.River);
                SeedTable<Calendar>("Calendar.json", _context.Calendar);
                SeedTable<Currency>("Currency.json", _context.Currency);
                SeedTable<Faction>("Factions.json", _context.Faction);
                SeedTable<FantasyDB.Models.Route>("Routes.json", _context.Route);
                SeedTable<Event>("Events.json", _context.Event);
                SeedTable<PriceExample>("PriceExamples.json", _context.PriceExample);
                SeedTable<Era>("Eras.json", _context.Era);
                SeedTable<Language>("Languages.json", _context.Language);
                SeedTable<CharacterRelationship>("CharacterRelationships.json", _context.CharacterRelationship);

                _context.SaveChanges();
                transaction.Commit();
                Console.WriteLine("✅ Seeding complete!");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine("❌ Seeding failed, all changes rolled back.");
                Console.WriteLine($"❗ Exception: {ex.Message}");

                var inner = ex.InnerException;
                while (inner != null)
                {
                    Console.WriteLine($"🔎 Inner Exception: {inner.Message}");
                    inner = inner.InnerException;
                }
            }

        }


        private void SeedTable<T>(string fileName, DbSet<T> dbSet) where T : class
        {
            Console.WriteLine($"🔄 Seeding {typeof(T).Name} from file: {fileName}"); // ✅ Moved here

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
}

