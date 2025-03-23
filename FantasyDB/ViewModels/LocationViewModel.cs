using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Models;
using FantasyDB.Services;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static FantasyDB.Models.JunctionClasses;

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
        // MULTIPLE CHILD LOCATIONS
        // --------------------------------------------
        [EditableForSnapshot]
        public List<int> ChildLocationIds { get; set; } = new();

        public List<string> ChildLocationNames { get; set; } = new();

        // --------------------------------------------
        // MULTIPLE EVENTS
        // --------------------------------------------
        [EditableForSnapshot]
        public List<int> EventIds { get; set; } = new();

        public List<string> EventNames { get; set; } = new();

        // --------------------------------------------
        // RELATIONSHIP FIELDS
        // --------------------------------------------
        [EditableForSnapshot]
        public int? ParentLocationId { get; set; }
        public string? ParentLocationName { get; set; } = string.Empty;

        [EditableForSnapshot]
        public int? SnapshotId { get; set; }
        public string? SnapshotName { get; set; } = string.Empty;

        [EditableForSnapshot]
        public int? LanguageId { get; set; }
        public string? LanguageName { get; set; } = string.Empty;
    }
}
