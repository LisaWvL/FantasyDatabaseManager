using System.ComponentModel.DataAnnotations.Schema;
namespace FantasyDB.Models // ✅ Add this line
{
    public class Calendar
    {
        public int Id { get; set; }
        public int? Day { get; set; }
        public string? Weekday { get; set; } = string.Empty; // Store weekdays as JSON
        public string? Month { get; set; } = string.Empty; // Store months as JSON

        public int? EventId { get; set; }
        [ForeignKey("EventId")]
        public Event? Event { get; set; }

    }
}

