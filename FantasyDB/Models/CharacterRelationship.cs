using System.Diagnostics.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FantasyDB.Models;
namespace FantasyDB.Models // ✅ Add this line
{
    public class CharacterRelationship
    {
        public int Id { get; set; }
        public int? Character1Id { get; set; }
        public int? Character2Id { get; set; }
        public string? RelationshipType { get; set; } = "Friend, Family, Ally, Sibling, Father, Mother, Mentor, Enemy, Archenemy";
        public string? RelationshipDynamic { get; set; }
        public int? SnapshotId { get; set; }
        // ✅ Navigation Properties (Add These Back)
        [NotMapped]
        public virtual Character? Character1 { get; set; }
        [NotMapped]
        public virtual Character? Character2 { get; set; }
        [NotMapped]
        public virtual Snapshot? Snapshot { get; set; }
    }
}