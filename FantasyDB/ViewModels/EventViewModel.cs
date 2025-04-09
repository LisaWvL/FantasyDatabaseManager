using FantasyDB.Attributes;
using FantasyDB.Interfaces;

namespace FantasyDB.ViewModels
{
    public class EventViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        [EditableForChapter]
        public string? Description { get; set; } = string.Empty;
        [EditableForChapter]
        public int? Day { get; set; }
        [EditableForChapter]
        public string? Month { get; set; } = string.Empty;
        [EditableForChapter]
        public int? Year { get; set; }
        public string? Purpose { get; set; } = string.Empty;
        [EditableForChapter]
        public int? ChapterId { get; set; }
        [EditableForChapter]
        public int? LocationId { get; set; }
        public int? ChapterNumber { get; set; } = 0;// Readable Name
        public string? LocationName { get; set; } = string.Empty;// Readable Name
    }
}
