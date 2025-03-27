using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FantasyDB.Models.JunctionClasses;

namespace FantasyDB.Models
{
    public class PlotPoint
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int? CalendarId { get; set; }
        public virtual Calendar? Calendar { get; set; }

        public int? SnapshotId { get; set; }
        public virtual Snapshot? Snapshot { get; set; }

        // Optional chapter override
        public string? BookOverride { get; set; }
        public string? ChapterOverride { get; set; }

        // Junctions
        public List<PlotPointCharacter> PlotPointCharacters { get; set; } = new();
        public List<PlotPointLocation> PlotPointLocations { get; set; } = new();
        public List<PlotPointEvent> PlotPointEvents { get; set; } = new();
        public List<PlotPointFaction> PlotPointFactions { get; set; } = new();
        public List<PlotPointItem> PlotPointItems { get; set; } = new();
        public List<PlotPointCharacterRelationship> PlotPointCharacterRelationships { get; set; } = new();
        public List<PlotPointEra> PlotPointEras { get; set; } = new();
        public List<PlotPointRiver> PlotPointRivers { get; set; } = new();
        public List<PlotPointRoute> PlotPointRoutes { get; set; } = new();

    }
}