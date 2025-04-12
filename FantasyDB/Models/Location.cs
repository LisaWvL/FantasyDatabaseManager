using System.Collections.Generic;  // Needed for ICollection, List, and Dictionary
using System.ComponentModel.DataAnnotations.Schema;
using static FantasyDB.Models.JunctionClasses;
namespace FantasyDB.Models // ✅ Add this line
{
    public class Location
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Type { get; set; }
        public string? Biome { get; set; }
        public string? Cultures { get; set; }
        public string? Politics { get; set; }
        public int? TotalPopulation { get; set; }
        public int? DivineMagicians { get; set; }
        public int? WildMagicians { get; set; }

        public int? ParentLocationId { get; set; }
        [ForeignKey("ParentLocationId")]
        public virtual Location? ParentLocation { get; set; }

        public int? ChapterId { get; set; }
        [ForeignKey("ChapterId")]
        public virtual Chapter? Chapter { get; set; }

        // ✅ Navigation for M:N with Language
        public List<LanguageLocation> LanguageLocations { get; set; } = [];

        // ✅ Navigation for 1:N with Event
        public List<Event> Events { get; set; } = [];

    }

}