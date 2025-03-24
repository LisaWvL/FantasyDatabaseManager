using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FantasyDB.Models
{
    public class JunctionClasses
    {

        public class LanguageLocation
        {
            public int? LocationId { get; set; }
            [ForeignKey("LocationId")]
            public Location? Location { get; set; }

            public int? LanguageId { get; set; }
            [ForeignKey("LanguageId")]
            public Language? Language { get; set; }
        }

        public class SnapshotCharacter
        {
            public int? SnapshotId { get; set; }

            [ForeignKey("SnapshotId")]
            public Snapshot? Snapshot { get; set; }

            public int? CharacterId { get; set; }

            [ForeignKey("CharacterId")]
            public Character? Character { get; set; }
        }

        public class SnapshotArtifact
        {
            public int? SnapshotId { get; set; }
            [ForeignKey("SnapshotId")]
            public Snapshot? Snapshot { get; set; }

            public int? ArtifactId { get; set; }

            [ForeignKey("ArtifactId")]
            public Artifact? Artifact { get; set; }
        }

        public class SnapshotEra
        {
            public int? SnapshotId { get; set; }
            [ForeignKey("SnapshotId")]

            public Snapshot? Snapshot { get; set; }

            public int? EraId { get; set; }
            [ForeignKey("EraId")]

            public Era? Era { get; set; }
        }

        public class SnapshotEvent
        {
            public int? SnapshotId { get; set; }
            [ForeignKey("SnapshotId")]

            public Snapshot? Snapshot { get; set; }

            public int? EventId { get; set; }

            [ForeignKey("EventId")]
            public Event? Event { get; set; }
        }

        public class SnapshotFaction
        {
            public int? SnapshotId { get; set; }

            [ForeignKey("SnapshotId")]
            public Snapshot? Snapshot { get; set; }

            public int? FactionId { get; set; }

            [ForeignKey("FactionId")]
            public Faction? Faction { get; set; }
        }

        public class SnapshotLocation
        {
            public int? SnapshotId { get; set; }
            [ForeignKey("SnapshotId")]
            public Snapshot? Snapshot { get; set; }

            public int? LocationId { get; set; }

            [ForeignKey("LocationId")]
            public Location? Location { get; set; }
        }

        public class SnapshotCharacterRelationship
        {
            public int? SnapshotId { get; set; }
            [ForeignKey("SnapshotId")]
            public Snapshot? Snapshot { get; set; }

            public int? CharacterRelationshipId { get; set; }
            [ForeignKey("CharacterRelationshipId")]
            public CharacterRelationship? CharacterRelationship { get; set; }
        }

        public class LocationLocation
        {
            public int? LocationId { get; set; }
            [ForeignKey("LocationId")]
            public Location? Location { get; set; }
            public int? ChildLocationId { get; set; }
            [ForeignKey("ChildLocationId")]
            public Location? ChildLocation { get; set; }
        }


        public class LocationEvent
        {
            public int? LocationId { get; set; }
            [ForeignKey("LocationId")]
            public Location? Location { get; set; }
            public int? EventId { get; set; }

            [ForeignKey("EventId")]
            public Event? Event { get; set; }
        }
    }
}