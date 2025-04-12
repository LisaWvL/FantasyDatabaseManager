using FantasyDB.Attributes;
using FantasyDB.Interfaces;

namespace FantasyDB.ViewModels
{
    public class EraViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        [EditableForChapter]
        public string? Description { get; set; } = string.Empty;
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        [EditableForChapter]
        public string? MagicSystem { get; set; } = string.Empty;
        [EditableForChapter]
        public string? MagicStatus { get; set; } = string.Empty;
        [EditableForChapter]
        public int? ChapterId { get; set; }
        public int? ChapterNumber { get; set; } = 0;
    }
}
