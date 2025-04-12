using System.ComponentModel.DataAnnotations.Schema;
namespace FantasyDB.Models // ✅ Add this line
{

    public class Event
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Purpose { get; set; }

        public int? ChapterId { get; set; }
        [ForeignKey("ChapterId")]

        public virtual Chapter? Chapter { get; set; }

        // ✅ FK to Location (1 Event → 1 Location)
        public int? LocationId { get; set; }
        [ForeignKey("LocationId")]
        public virtual Location? Location { get; set; } = default!;

        public int? CalendarId { get; set; }
        [ForeignKey("CalendarId")]

        public virtual Calendar? Date { get; set; } = default!;

    }

}

