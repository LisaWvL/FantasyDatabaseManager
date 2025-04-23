using System;
using FantasyDB.Services;
using Microsoft.EntityFrameworkCore;
using FantasyDB.Entities._Shared;


namespace FantasyDB.Entities
{
    public class DatabaseResetService
    {
        private readonly AppDbContext _context;
        private readonly JsonSeeder _jsonSeeder;
        private readonly SeedFks _seedFks;

        public DatabaseResetService(AppDbContext context, JsonSeeder jsonSeeder, SeedFks seedFks)
        {
            _context = context;
            _jsonSeeder = jsonSeeder;
            _seedFks = seedFks;
        }

        public void RunInteractive()
        {
            Console.WriteLine("🌍 Welcome to the FantasyDB Reset Tool");
            Console.WriteLine("─────────────────────────────────────");

            if (Ask("⚠️  Do you want to DROP and RECREATE the database? (y/n)"))
                RecreateDatabase();

            if (Ask("🌱 Do you want to SEED the database with base data? (y/n)"))
                _jsonSeeder.Seed();

            if (Ask("🔗 Do you want to ADD FOREIGN KEYS from FK data files? (y/n)"))
                _seedFks.UpdateForeignKeys();

            Console.WriteLine("\n🏁 All steps completed. Press any key to exit...");
            Console.ReadKey();
        }

        public void RecreateDatabase()
        {
            Console.WriteLine("⚠️  Dropping and recreating the entire database...");
            try
            {
                _context.Database.EnsureDeleted();
                Console.WriteLine("🧨 Database dropped.");

                _context.Database.Migrate();
                Console.WriteLine("✅ Database recreated with current schema.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to recreate database: {ex.Message}");
                LogInnerExceptions(ex);
            }
        }

        private static bool Ask(string question)
        {
            Console.Write($"{question} ");
            var response = Console.ReadLine()?.Trim().ToLower();
            return response == "y" || response == "yes";
        }

        private static void LogInnerExceptions(Exception ex)
        {
            var inner = ex.InnerException;
            while (inner != null)
            {
                Console.WriteLine($"🔎 Inner: {inner.Message}");
                inner = inner.InnerException;
            }
        }
    }
}
