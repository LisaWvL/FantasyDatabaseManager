namespace FantasyDB.Models // ✅ Add this line
{

    public class Currency
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Crown { get; set; } = 1;
        public int? Shilling { get; set; } = 20;
        public int? Penny { get; set; } = 240;

    }
}
