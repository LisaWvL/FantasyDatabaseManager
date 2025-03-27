using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Attributes;
using FantasyDB.Interfaces;

namespace FantasyDB.ViewModels
{
    public class PlotPointViewModel : IViewModelWithId
    {
        public int Id { get; set; }

        [EditableForSnapshot]
        public string Title { get; set; } = string.Empty;
        [EditableForSnapshot]
        public string? Description { get; set; }

        [EditableForSnapshot]
        public int? CalendarId { get; set; }
        public string? CalendarLabel { get; set; } = string.Empty;

        [EditableForSnapshot]
        public int? SnapshotId { get; set; }
        public string? SnapshotName { get; set; } = string.Empty;

        // Optional overrides (e.g. visual grouping)
        public string? BookOverride { get; set; }
        public string? ChapterOverride { get; set; }

        // Junction References
        [HandlesJunction("PlotPointCharacter", "PlotPointId", "CharacterId")]
        public List<int> CharacterIds { get; set; } = new();
        public List<string> CharacterNames { get; set; } = new();

        [HandlesJunction("PlotPointLocation", "PlotPointId", "LocationId")]
        public List<int> LocationIds { get; set; } = new();
        public List<string> LocationNames { get; set; } = new();

        [HandlesJunction("PlotPointEvent", "PlotPointId", "EventId")]
        public List<int> EventIds { get; set; } = new();
        public List<string> EventNames { get; set; } = new();

        [HandlesJunction("PlotPointFaction", "PlotPointId", "FactionId")]
        public List<int> FactionIds { get; set; } = new();
        public List<string> FactionNames { get; set; } = new();

        [HandlesJunction("PlotPointItem", "PlotPointId", "ItemId")]
        public List<int> ItemIds { get; set; } = new();
        public List<string> ItemNames { get; set; } = new();

        [HandlesJunction("PlotPointCharacterRelationship", "PlotPointId", "CharacterRelationshipId")]
        public List<int> CharacterRelationshipIds { get; set; } = new();
        public List<string> CharacterRelationshipNames { get; set; } = new();

        [HandlesJunction("PlotPointEra", "PlotPointId", "EraId")]
        public List<int> EraIds { get; set; } = new();
        public List<string> EraNames { get; set; } = new();

        [HandlesJunction("PlotPointRiver", "PlotPointId", "RiverId")]
        public List<int> RiverIds { get; set; } = new();
        public List<string> RiverNames { get; set; } = new();

        [HandlesJunction("PlotPointRoute", "PlotPointId", "RouteId")]
        public List<int> RouteIds { get; set; } = new();
        public List<string> RouteNames { get; set; } = new();

    }
}