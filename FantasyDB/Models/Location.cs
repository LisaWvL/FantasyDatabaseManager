using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Collections.Generic;  // Needed for ICollection, List, and Dictionary
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public int? ChildLocationId { get; set; }
        public int? ParentLocationId { get; set; }
        public int? EventId { get; set; }
        public int? LanguageId { get; set; }
        public int? SnapshotId { get; set; }
        // ✅ Computed Property for Display
        [NotMapped]
        public string DisplayName => $"{Type} - {Name}";

        // ✅ Navigation Properties (Add These Back)
        [NotMapped]
        public virtual Location? ChildLocation { get; set; }
        [NotMapped]
        public virtual Location? ParentLocation { get; set; }
        [NotMapped]
        public virtual Event? Event { get; set; }
        [NotMapped]
        public virtual Language? Language { get; set; }
        [NotMapped]
        public virtual Snapshot? Snapshot { get; set; }

    }
}