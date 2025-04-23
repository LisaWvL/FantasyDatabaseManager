using FantasyDB.Attributes;
using FantasyDB.Entities._Shared;
using FantasyDB.Entities;



namespace FantasyDB.Entities
{
    public class EraViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        [EditableForChapter]
        public string? Description { get; set; } = string.Empty;
        [EditableForChapter]
        public string? MagicSystem { get; set; } = string.Empty;
        [EditableForChapter]
        public string? MagicStatus { get; set; } = string.Empty;

        public int? ChapterNumber { get; set; } = 0;

        [EditableForChapter]
        public int? ChapterId { get; set; } = 0;
        [EditableForChapter]

        public int? StartDateId { get; set; } = 0;
        [EditableForChapter]

        public int? EndDateId { get; set; } = 0;
    }
}

