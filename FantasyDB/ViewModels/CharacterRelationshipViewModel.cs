using FantasyDB.Attributes;
using FantasyDB.Interfaces;

namespace FantasyDB.ViewModels
{
    public class CharacterRelationshipViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public string? Character1Name { get; set; } = string.Empty;// Readable Name
        public string? Character2Name { get; set; } = string.Empty;// Readable Name
        [EditableForChapter]
        public string? RelationshipType { get; set; } = "Friend, Family, Ally, Sibling, Father, Mother, Mentor, Enemy, Archenemy";
        [EditableForChapter]
        public string? RelationshipDynamic { get; set; } = string.Empty;
        public int? Character1Id { get; set; }
        public int? Character2Id { get; set; }
        [EditableForChapter]
        public int? ChapterId { get; set; }
        public int? ChapterNumber { get; set; } = 0;
    }
}
