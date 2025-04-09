using FantasyDB.Attributes;
using FantasyDB.Interfaces;

namespace FantasyDB.ViewModels
{
    public class FactionViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Alias { get; set; } = string.Empty;
        public int? FoundingYear { get; set; }
        [EditableForChapter]
        public string? Magic { get; set; } = string.Empty;
        [EditableForChapter]
        public int? ChapterId { get; set; }
        public int? FounderId { get; set; }
        [EditableForChapter]
        public int? LeaderId { get; set; }
        [EditableForChapter]
        public int? HQLocationId { get; set; }
        public string? FounderName { get; set; } = string.Empty;// Readable Name
        public string? LeaderName { get; set; } = string.Empty;// Readable Name
        public string? HQLocationName { get; set; } = string.Empty;
        public int? ChapterNumber { get; set; } = 0;
    }
}
