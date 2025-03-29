using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Models;
using static FantasyDB.Models.JunctionClasses;

namespace FantasyDB.Services
{
    public class SeedFks
    {
        private readonly AppDbContext _context;

        public SeedFks(AppDbContext context)
        {
            _context = context;
        }

        public void UpdateForeignKeys()
        {
            Console.WriteLine("🔄 Updating foreign keys from FK data files...");
            var basePath = @"F:\FantasyDatabase\FantasyDatabaseManager\FantasyDB\Seed Data with FKs\";

            Update<Snapshot>(basePath + "Snapshots.json");
            Update<Language>(basePath + "Languages.json");
            Update<Location>(basePath + "Locations.json");
            Update<Character>(basePath + "Characters.json");
            Update<Faction>(basePath + "Factions.json");
            Update<Item>(basePath + "Items.json");
            Update<Calendar>(basePath + "Calendar.json");
            Update<Event>(basePath + "Events.json");
            Update<Era>(basePath + "Eras.json");
            Update<Currency>(basePath + "Currencies.json");
            Update<Route>(basePath + "Routes.json");
            Update<River>(basePath + "Rivers.json");
            Update<PriceExample>(basePath + "PriceExamples.json");

            // Junctions
            Update<LanguageLocation>(basePath + "LanguagesLocations.json");

            Update<SnapshotCharacter>(basePath + "SnapshotsCharacters.json");
            Update<SnapshotItem>(basePath + "SnapshotsItems.json");
            Update<SnapshotEra>(basePath + "SnapshotsEras.json");
            Update<SnapshotEvent>(basePath + "SnapshotsEvents.json");
            Update<SnapshotFaction>(basePath + "SnapshotsFactions.json");
            Update<SnapshotLocation>(basePath + "SnapshotsLocations.json");
            Update<SnapshotCharacterRelationship>(basePath + "SnapshotCharacterRelationships.json");

            Update<CharacterRelationship>(basePath + "CharacterRelationships.json");
            Update<PlotPoint>(basePath + "PlotPoints.json");
            Update<PlotPointRiver>(basePath + "PlotPointsRivers.json");
            Update<PlotPointRoute>(basePath + "PlotPointsRoutes.json");

            _context.SaveChanges();
            Console.WriteLine("✅ Foreign key updates complete.");
        }

        private void Update<T>(string filePath) where T : class
        {
            var entityName = typeof(T).Name;
            Console.WriteLine($"📂 Processing FK file: {Path.GetFileName(filePath)}");

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"❌ [MISSING] FK file not found: {filePath}");
                return;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                var json = File.ReadAllText(filePath);
                var updates = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(json, options);

                if (updates == null || !updates.Any())
                {
                    Console.WriteLine($"⚠️ [EMPTY] No FK data in {filePath}");
                    return;
                }

                var dbSet = _context.Set<T>();
                int processed = 0;

                foreach (var row in updates)
                {
                    if (row.ContainsKey("Id"))
                    {
                        // Base entity: Update by ID
                        var id = row["Id"].GetInt32();
                        var entity = dbSet.Find(id);
                        if (entity == null) continue;

                        foreach (var kvp in row.Where(kv => kv.Key != "Id"))
                        {
                            var prop = entity.GetType().GetProperty(kvp.Key);
                            if (prop != null)
                            {
                                var value = ConvertJsonElement(kvp.Value, prop.PropertyType);
                                prop.SetValue(entity, value);
                            }
                        }
                    }
                    else
                    {
                        // Junction table: Create new instance
                        var instance = Activator.CreateInstance<T>();
                        foreach (var kvp in row)
                        {
                            var prop = instance.GetType().GetProperty(kvp.Key);
                            if (prop != null)
                            {
                                var value = ConvertJsonElement(kvp.Value, prop.PropertyType);
                                prop.SetValue(instance, value);
                            }
                        }

                        dbSet.Add(instance);
                    }

                    processed++;
                }

                Console.WriteLine($"  → Processed {processed} rows in {entityName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ [ERROR] Failed to process {filePath}");
                Console.WriteLine($"   {ex.Message}");
                var inner = ex.InnerException;
                while (inner != null)
                {
                    Console.WriteLine($"🔎 Inner: {inner.Message}");
                    inner = inner.InnerException;
                }
            }
        }

        private object? ConvertJsonElement(JsonElement jsonElement, Type targetType)
        {
            if (jsonElement.ValueKind == JsonValueKind.Null)
                return null;

            try
            {
                if (targetType == typeof(string)) return jsonElement.GetString();
                if (targetType == typeof(int)) return jsonElement.GetInt32();
                if (targetType == typeof(int?)) return jsonElement.ValueKind == JsonValueKind.Null ? null : jsonElement.GetInt32();
                if (targetType == typeof(bool)) return jsonElement.GetBoolean();
                if (targetType == typeof(DateTime)) return jsonElement.GetDateTime();

                // Fallback for other types (like enums or floats)
                return JsonSerializer.Deserialize(jsonElement.GetRawText(), targetType);
            }
            catch
            {
                Console.WriteLine($"⚠️ Warning: Failed to convert '{jsonElement}' to {targetType.Name}");
                return null;
            }
        }
    }
}
