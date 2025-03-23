using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Collections.Generic;  // Needed for ICollection, List, and Dictionary
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Services;
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
        public int? LanguageId { get; set; }
        public int? SnapshotId { get; set; }
         
        public virtual Location? ParentLocation { get; set; }
         
        public virtual Language? Language { get; set; }
         
        public virtual Snapshot? Snapshot { get; set; }

        // --------------------------------------------
        // MULTIPLE CHILD LOCATIONS AND EVENTS
        // --------------------------------------------
         
        public List<Location> ChildLocations { get; set; } = new();
         
        public List<Event> Events { get; set; } = new();
    }
}