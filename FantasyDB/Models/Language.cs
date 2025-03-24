using System;
using System.Collections.Generic;  // Needed for ICollection, List, and Dictionary
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FantasyDB.Models.JunctionClasses;
namespace FantasyDB.Models // ✅ Add this line
{
    public class Language
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty;
        public string? Text { get; set; } = string.Empty;
        // ✅ Navigation Properties (Add These Back)

        public List<LanguageLocation> LanguageLocations { get; set; } = new();

    }
}