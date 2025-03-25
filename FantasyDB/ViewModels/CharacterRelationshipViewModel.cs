using FantasyDB.Attributes;
using FantasyDB.Interfaces;

namespace FantasyDB.ViewModels
{
    public class CharacterRelationshipViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public string? Character1Name { get; set; } = string.Empty;// Readable Name
        public string? Character2Name { get; set; } = string.Empty;// Readable Name
        [EditableForSnapshot]
        public string? RelationshipType { get; set; } = "Friend, Family, Ally, Sibling, Father, Mother, Mentor, Enemy, Archenemy";
        [EditableForSnapshot]
        public string? RelationshipDynamic { get; set; } = string.Empty;
        public int? Character1Id { get; set; }
        public int? Character2Id { get; set; }
        [EditableForSnapshot]
        public int? SnapshotId { get; set; }
        public string? SnapshotName { get; set; } = string.Empty;
    }
}
