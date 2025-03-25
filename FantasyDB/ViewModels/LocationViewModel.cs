using System.Collections.Generic;
using FantasyDB.Attributes;
using FantasyDB.Interfaces;

namespace FantasyDB.ViewModels
{
    public class LocationViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty;
        public string? Biome { get; set; } = string.Empty;
        [EditableForSnapshot]
        public string? Cultures { get; set; } = string.Empty;
        [EditableForSnapshot]
        public string? Politics { get; set; } = string.Empty;
        [EditableForSnapshot]
        public int? TotalPopulation { get; set; }
        [EditableForSnapshot]
        public int? DivineMagicians { get; set; }
        [EditableForSnapshot]
        public int? WildMagicians { get; set; }


        // --------------------------------------------
        // MULTIPLE EVENTS AND LANGUAGES
        // --------------------------------------------
        [EditableForSnapshot]
        public List<int> EventIds { get; set; } = new();
        public List<string> EventNames { get; set; } = new();

        [EditableForSnapshot]
        public List<int> LanguageIds { get; set; } = new();
        public List<string>? LanguageNames { get; set; } = new();

        // --------------------------------------------
        // RELATIONSHIP FIELDS
        // --------------------------------------------
        [EditableForSnapshot]
        public int? ParentLocationId { get; set; }
        public string? ParentLocationName { get; set; } = string.Empty;

        [EditableForSnapshot]
        public int? SnapshotId { get; set; }
        public string? SnapshotName { get; set; } = string.Empty;


    }
}
