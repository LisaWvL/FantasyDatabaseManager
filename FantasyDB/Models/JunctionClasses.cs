using System.ComponentModel.DataAnnotations.Schema;

namespace FantasyDB.Models
{
    public class JunctionClasses
    {

        public class LanguageLocation
        {
            public int LanguageId { get; set; }
            public Language Language { get; set; } = default!;

            public int LocationId { get; set; }
            public Location Location { get; set; } = default!;
        }


        //public class LocationEvent
        //{
        //    public int LocationId { get; set; }
        //    public Location Location { get; set; }
        //    public int EventId { get; set; }
        //    public Event Event { get; set; }
        //}


        public class SnapshotCharacter
        {
            public int SnapshotId { get; set; }

            public Snapshot Snapshot { get; set; }

            public int CharacterId { get; set; }

            public Character Character { get; set; }
        }

        public class SnapshotItem
        {
            public int SnapshotId { get; set; }
            public Snapshot Snapshot { get; set; }

            public int ItemId { get; set; }

            public Item Item { get; set; }
        }

        public class SnapshotEra
        {
            public int SnapshotId { get; set; }

            public Snapshot Snapshot { get; set; }

            public int EraId { get; set; }

            public Era Era { get; set; }
        }

        public class SnapshotEvent
        {
            public int SnapshotId { get; set; }

            public Snapshot Snapshot { get; set; }

            public int EventId { get; set; }

            public Event Event { get; set; }
        }

        public class SnapshotFaction
        {
            public int SnapshotId { get; set; }

            public Snapshot Snapshot { get; set; }

            public int FactionId { get; set; }

            public Faction Faction { get; set; }
        }

        public class SnapshotLocation
        {
            public int SnapshotId { get; set; }
            public Snapshot Snapshot { get; set; }

            public int LocationId { get; set; }
            public Location Location { get; set; }
        }

        public class SnapshotCharacterRelationship
        {
            public int SnapshotId { get; set; }
            public Snapshot Snapshot { get; set; }

            public int CharacterRelationshipId { get; set; }
            public CharacterRelationship CharacterRelationship { get; set; }
        }

        public class PlotPointRiver
        {
            public int PlotPointId { get; set; }
            public PlotPoint PlotPoint { get; set; }

            public int RiverId { get; set; }
            public River River { get; set; }
        }

        public class PlotPointRoute
        {
            public int PlotPointId { get; set; }
            public PlotPoint PlotPoint { get; set; }

            public int RouteId { get; set; }
            public Models.Route Route { get; set; }
        }
    }
}