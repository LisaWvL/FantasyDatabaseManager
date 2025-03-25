
// File: DbSeedCommand.cs
using System;

namespace FantasyDB // 👈 Or FantasyDB, if you want to move it
{
    public static class DbSeedCommand
    {
        public static void RunJsonSeed()
        {
            Console.WriteLine("📦 Triggering manual JSON seed...");

            var factory = new AppDbContextFactory();
            using var context = factory.CreateDbContext(Array.Empty<string>());

            var seeder = new JsonSeeder(context);
            seeder.Seed();

            Console.WriteLine("✅ Done seeding database.");

            Console.WriteLine("\nPress any key to close...");
            Console.ReadKey(); // ⏳ Waits for a key press so console doesn’t close
        }


    }
}
