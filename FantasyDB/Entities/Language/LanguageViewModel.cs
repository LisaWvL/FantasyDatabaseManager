using System.Collections.Generic;
using FantasyDB.Attributes;
using FantasyDB.Entities._Shared;
using FantasyDB.Entities;


namespace FantasyDB.Entities
{
    public class LanguageViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public string? Type { get; set; } = string.Empty;
        public string? Text { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;

        // --------------------------------------------
        // MULTIPLE Locations where this language is spoken 
        // --------------------------------------------
        [EditableForChapter]
        [HandlesJunction("LanguageLocation", "LanguageId", "LocationId")]
        public List<int> LocationIds { get; set; } = [];

        public List<string> LocationNames { get; set; } = [];
    }
}
