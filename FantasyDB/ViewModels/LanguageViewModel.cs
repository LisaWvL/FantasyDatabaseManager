using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Models;
using FantasyDB.Services;

namespace FantasyDB.ViewModels
{
    public class LanguageViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public string? Type { get; set; } = string.Empty;
        public string? Text { get; set; } = string.Empty;
        public int? LocationId { get; set; }
        public string? LocationName { get; set; } = string.Empty;
        public string? Name { get; set; } =string.Empty;

        // --------------------------------------------
        // MULTIPLE Locations where this language is spoken 
        // --------------------------------------------
        [EditableForSnapshot]
        public List<int> LocationIds { get; set; } = new();

        public List<string> LocationNames { get; set; } = new();
    }
}
