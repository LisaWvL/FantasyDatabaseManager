using FantasyDB.Attributes;
using FantasyDB.Entities._Shared;
using FantasyDB.Entities;



namespace FantasyDB.Entities
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

        public int? StartDateId { get; set; } = 0;
        [EditableForChapter]

        public int? EndDateId { get; set; } = 0;
    }

}
