using System.Collections.Generic;  // Needed for ICollection, List, and Dictionary
using static FantasyDB.Models.JunctionClasses;
namespace FantasyDB.Models // ✅ Add this line
{
    public class Language
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty;
        public string? Text { get; set; } = string.Empty;

        // ✅ Navigation for M:N with Location
        // Language.cs
        public List<LanguageLocation> LanguageLocations { get; set; } = [];

    }

}