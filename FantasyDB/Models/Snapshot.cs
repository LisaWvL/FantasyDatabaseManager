namespace FantasyDB.Models // ✅ Add this line
{
    public class Snapshot
    {
        public int Id { get; set; }
        public string? Book { get; set; } = string.Empty;
        public string? Act { get; set; } = string.Empty;
        public string? Chapter { get; set; } = string.Empty;
        public string? SnapshotName { get; set; } = string.Empty; // ✅ now loaded from DB

    }
}