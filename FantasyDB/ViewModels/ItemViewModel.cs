using FantasyDB.Attributes;
using FantasyDB.Interfaces;

namespace FantasyDB.ViewModels
{
    public class ItemViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Origin { get; set; } = string.Empty;
        public string? Effects { get; set; } = string.Empty;
        [EditableForChapter]
        public int? ChapterId { get; set; }
        [EditableForChapter]
        public int? OwnerId { get; set; }
        public string? OwnerName { get; set; } = string.Empty; // Readable Name
        public int? ChapterNumber { get; set; } = 0;
    }
}
