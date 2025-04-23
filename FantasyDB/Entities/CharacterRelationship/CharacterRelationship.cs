using System.ComponentModel.DataAnnotations.Schema;
using FantasyDB.Entities._Shared;
using FantasyDB.Entities;


namespace FantasyDB.Entities // ✅ Add this line
{
    public class CharacterRelationship
    {
        public int Id { get; set; }
        public int? Character1Id { get; set; }
        public int? Character2Id { get; set; }
        public string? RelationshipType { get; set; } = "Friend, Family, Ally, Sibling, Father, Mother, Mentor, Enemy, Archenemy";
        public string? RelationshipDynamic { get; set; }
        public int? ChapterId { get; set; }
        // ✅ Navigation Properties (Add These Back)
        public virtual Character? Character1 { get; set; }
        public virtual Character? Character2 { get; set; }
        public virtual Chapter? Chapter { get; set; }
    }
}