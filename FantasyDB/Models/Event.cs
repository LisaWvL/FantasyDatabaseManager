using System.ComponentModel.DataAnnotations.Schema;
namespace FantasyDB.Models // ✅ Add this line
{

    public class Event
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? Day { get; set; }
        public string? Month { get; set; }
        public int? Year { get; set; }
        public string? Purpose { get; set; }

        public int? SnapshotId { get; set; }
        public virtual Snapshot? Snapshot { get; set; }

        // ✅ FK to Location (1 Event → 1 Location)
        public int? LocationId { get; set; }
        public virtual Location? Location { get; set; } = default!;


    }

}

