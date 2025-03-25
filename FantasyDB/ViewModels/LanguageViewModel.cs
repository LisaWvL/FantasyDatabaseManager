using System.Collections.Generic;
using FantasyDB.Attributes;
using FantasyDB.Interfaces;

namespace FantasyDB.ViewModels
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
        [EditableForSnapshot]
        [HandlesJunction("LanguageLocation", "LanguageId", "LocationId")]
        public List<int> LocationIds { get; set; } = new();

        public List<string> LocationNames { get; set; } = new();
    }
}
