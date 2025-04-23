using FantasyDB.Entities._Shared;
using FantasyDB.Entities;


namespace FantasyDB.Entities // ✅ Add this line
{
    public class PriceExample
    {
        public int Id { get; set; }
        public string? Category { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Exclusivity { get; set; }
        public int? Price { get; set; }
    }
}