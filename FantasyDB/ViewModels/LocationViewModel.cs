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
        [EditableForChapter]
        public string? Cultures { get; set; } = string.Empty;
        [EditableForChapter]
        public string? Politics { get; set; } = string.Empty;
        [EditableForChapter]
        public int? TotalPopulation { get; set; }
        [EditableForChapter]
        public int? DivineMagicians { get; set; }
        [EditableForChapter]
        public int? WildMagicians { get; set; }


        // --------------------------------------------
        // MULTIPLE EVENTS AND LANGUAGES
        // --------------------------------------------
        [EditableForChapter]
        public List<int> EventIds { get; set; } = [];
        public List<string> EventNames { get; set; } = [];

        [EditableForChapter]
        public List<int> LanguageIds { get; set; } = [];
        public List<string>? LanguageNames { get; set; } = [];

        // --------------------------------------------
        // RELATIONSHIP FIELDS
        // --------------------------------------------
        [EditableForChapter]
        public int? ParentLocationId { get; set; }
        public string? ParentLocationName { get; set; } = string.Empty;

        [EditableForChapter]
        public int? ChapterId { get; set; }
        public int? ChapterNumber { get; set; } = 0;


    }
}
