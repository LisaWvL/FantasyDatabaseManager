using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Models;

namespace FantasyDB.ViewModels
{
    public class LocationViewModel
    {
        public string? Name { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty;
        public string? Biome { get; set; } = string.Empty;
        public string? Cultures { get; set; } = string.Empty;
        public string? Politics { get; set; } = string.Empty;
        public int? TotalPopulation { get; set; }
        public int? DivineMagicians { get; set; }
        public int? WildMagicians { get; set; }
        [NotMapped]
        public string? ChildLocationName { get; set; } = string.Empty;
        [NotMapped]
        public string? ParentLocationName { get; set; } = string.Empty;
        [NotMapped]
        public string? EventName { get; set; } = string.Empty;
        [NotMapped]
        public string? SnapshotName { get; set; } = string.Empty;
        [NotMapped]
        public string? LanguageName { get; set; } = string.Empty;
        [NotMapped]
        public string? HQLocationName { get; set; } = string.Empty;
    }
}
