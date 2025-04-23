using System.ComponentModel.DataAnnotations.Schema;

using FantasyDB.Entities._Shared;
using FantasyDB.Entities;



namespace FantasyDB.Entities // ✅ Add this line
{
    public class Date
    {
        public int Id { get; set; }
        public int? Day { get; set; }
        public string? Weekday { get; set; } = string.Empty; // Store weekdays as JSON
        public string? Month { get; set; } = string.Empty; // Store months as JSON
        public int? Year { get; set; } = 0; // Store year as JSON

    }

}

