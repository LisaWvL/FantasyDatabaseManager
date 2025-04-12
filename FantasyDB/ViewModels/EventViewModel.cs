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
        public string? Purpose { get; set; } = string.Empty;
        [EditableForChapter]
        public int? ChapterId { get; set; }
        [EditableForChapter]
        public int? LocationId { get; set; }
        public string? LocationName { get; set; } = string.Empty;// Readable Name
        [EditableForChapter]
        public int? CalendarId { get; set; } = 0;// Readable Name
        public string? ReadableDate { get; set; } = string.Empty;// Readable Name
    }
}
