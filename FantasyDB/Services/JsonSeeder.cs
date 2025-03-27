using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FantasyDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using static FantasyDB.Models.JunctionClasses;

namespace FantasyDB
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
            Console.WriteLine($"📂 Current Directory: {Directory.GetCurrentDirectory()}");

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // ORDER MATTERS: parents first, then junctions
                Seed<Snapshot>("Snapshots.json");
                Seed<Language>("Languages.json");
                Seed<Location>("Locations.json");
                Seed<Character>("Characters.json");
                Seed<Faction>("Factions.json");
                Seed<Item>("Items.json");
                Seed<Calendar>("Calendar.json");
                Seed<Event>("Events.json");
                Seed<Era>("Eras.json");
                Seed<Currency>("Currencies.json");
                Seed<Route>("Routes.json");
                Seed<River>("Rivers.json");
                Seed<PriceExample>("PriceExamples.json");

                // Junctions & Complex Links
                Seed<LanguageLocation>("LanguagesLocations.json");

                Seed<SnapshotCharacter>("SnapshotsCharacters.json");
                Seed<SnapshotItem>("SnapshotsItems.json");
                Seed<SnapshotEra>("SnapshotsEras.json");
                Seed<SnapshotEvent>("SnapshotsEvents.json");
                Seed<SnapshotFaction>("SnapshotsFactions.json");
                Seed<SnapshotLocation>("SnapshotsLocations.json");
                Seed<SnapshotCharacterRelationship>("SnapshotsCharacterRelationships.json");

                Seed<CharacterRelationship>("CharacterRelationships.json");
                Seed<PlotPoint>("PlotPoints.json");
                Seed<PlotPointCharacter>("PlotPointsCharacters.json");
                Seed<PlotPointLocation>("PlotPointsLocations.json");
                Seed<PlotPointEvent>("PlotPointsEvents.json");
                Seed<PlotPointFaction>("PlotPointsFactions.json");
                Seed<PlotPointItem>("PlotPointsItems.json");
                Seed<PlotPointEra>("PlotPointsEras.json");
                Seed<PlotPointCharacterRelationship>("PlotPointsCharacterRelationships.json");
                Seed<PlotPointRiver>("PlotPointsRivers.json");
                Seed<PlotPointRoute>("PlotPointsRoutes.json");

                _context.SaveChanges();
                transaction.Commit();
                Console.WriteLine("✅ Seeding complete!");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine("❌ Seeding failed, all changes rolled back.");
                LogException(ex);
            }
        }

        private void ResetIdentity<T>() where T : class
        {
            var entityType = _context.Model.FindEntityType(typeof(T));
            var tableName = entityType?.GetTableName();

            if (string.IsNullOrWhiteSpace(tableName))
            {
                Console.WriteLine($"❌ [RESEED SKIPPED] Could not resolve table name for {typeof(T).Name}");
                return;
            }

            try
            {
                // Always reseed to 1, so first insert = 1
                var sql = $"DBCC CHECKIDENT ('[{tableName}]', RESEED, 1);";
                _context.Database.ExecuteSqlRaw(sql);
                Console.WriteLine($"🔁 Reseeded identity for [{tableName}] to 1 (next ID = 2)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ [ERROR] Could not reseed identity for [{tableName}]");
                LogException(ex);
            }
        }



        private void Seed<T>(string filename) where T : class
        {
            var entityName = typeof(T).Name;
            var fullPath = Path.Combine("F:\\FantasyDatabase\\FantasyDatabaseManager\\FantasyDB\\Seed Data\\", filename);
            var tableName = _context.Model.FindEntityType(typeof(T))?.GetTableName();

            Console.WriteLine($"\n🔄 Seeding {entityName} from file: {filename}");
            if (!string.IsNullOrEmpty(tableName))
                Console.WriteLine($"📦 Mapped to table: {tableName}");

            if (!File.Exists(fullPath))
            {
                Console.WriteLine($"❌ [MISSING] File not found: {fullPath}");
                return;
            }

            var jsonData = File.ReadAllText(fullPath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new StringOrArrayConverter() }
            };

            try
            {
                var data = JsonSerializer.Deserialize<List<T>>(jsonData, options);
                if (data == null || !data.Any())
                {
                    Console.WriteLine($"⚠️ [EMPTY] No data found in {filename}, skipping...");
                    return;
                }

                var dbSet = _context.Set<T>();

                if (!dbSet.Any())
                {
                    ResetIdentity<T>();
                    dbSet.AddRange(data);
                    Console.WriteLine($"  → Inserted {data.Count} rows into {tableName}");
                }
                else
                {
                    Console.WriteLine($"ℹ️ {tableName} already has data. Skipping...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ [ERROR] Failed to process {filename}");
                LogException(ex);
            }
        }

        private void LogException(Exception ex)
        {
            Console.WriteLine($"❗ {ex.Message}");
            var inner = ex.InnerException;
            while (inner != null)
            {
                Console.WriteLine($"🔎 Inner: {inner.Message}");
                inner = inner.InnerException;
            }
        }
    }
}
