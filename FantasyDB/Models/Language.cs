using System;
using System.Collections.Generic;  // Needed for ICollection, List, and Dictionary
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FantasyDB.Models // ✅ Add this line
{
    public class Language
    {
        public int Id { get; set; }
        public string? Type { get; set; } = string.Empty;
        public string? Text { get; set; }
        public int? LocationId { get; set; }
        // ✅ Navigation Properties (Add These Back)
        public virtual Location? Location { get; set; }

        // ✅ Computed Property for Display
        [NotMapped]
        public string? LanguageName { get; set; }

    }
}