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
            public Location? Location { get; set; }

            public int? LanguageId { get; set; }
            public Language? Language { get; set; }
        }

        public class SnapshotCharacter
        {
            public int? SnapshotId { get; set; }
             
            public Snapshot? Snapshot { get; set; }

            public int? CharacterId { get; set; }
             
            public Character? Character { get; set; }
        }

        public class SnapshotArtifact
        {
            public int? SnapshotId { get; set; }
             
            public Snapshot? Snapshot { get; set; }

            public int? ArtifactId { get; set; }
             
            public Artifact? Artifact { get; set; }
        }

        public class SnapshotEra
        {
            public int? SnapshotId { get; set; }
             
            public Snapshot? Snapshot { get; set; }

            public int? EraId { get; set; }
             
            public Era? Era { get; set; }
        }

        public class SnapshotEvent
        {
            public int? SnapshotId { get; set; }
             
            public Snapshot? Snapshot { get; set; }

            public int? EventId { get; set; }
             
            public Event? Event { get; set; }
        }

        public class SnapshotFaction
        {
            public int? SnapshotId { get; set; }
             
            public Snapshot? Snapshot { get; set; }

            public int? FactionId { get; set; }
             
            public Faction? Faction { get; set; }
        }

        public class SnapshotLocation
        {
            public int? SnapshotId { get; set; }
             
            public Snapshot? Snapshot { get; set; }

            public int? LocationId { get; set; }
             
            public Location? Location { get; set; }
        }

        public class SnapshotCharacterRelationship
        {
            public int? SnapshotId { get; set; }
             
            public Snapshot? Snapshot { get; set; }

            public int? CharacterRelationshipId { get; set; }
             
            public CharacterRelationship? CharacterRelationship { get; set; }
        }

        public class LocationLocation
        {
            public int? LocationId { get; set; }
             
            public Location? Location { get; set; }
            public int? ChildLocationId { get; set; }
             
            public Location? ChildLocation { get; set; }
        }


        public class LocationEvent
        {
            public int? LocationId { get; set; }
             
            public Location? Location { get; set; }
            public int? EventId { get; set; }
             
            public Event? Event { get; set; }
        }
    }
}