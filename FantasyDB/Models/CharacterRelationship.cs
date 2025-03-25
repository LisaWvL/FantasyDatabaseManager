using System.ComponentModel.DataAnnotations.Schema;
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
        [ForeignKey("Character1Id")]
        public virtual Character? Character1 { get; set; }
        [ForeignKey("Character2Id")]
        public virtual Character? Character2 { get; set; }
        [ForeignKey("SnapshotId")]
        public virtual Snapshot? Snapshot { get; set; }
    }
}