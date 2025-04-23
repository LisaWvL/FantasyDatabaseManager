using FantasyDB.Attributes;
using FantasyDB.Entities._Shared;
using FantasyDB.Entities;



namespace FantasyDB.Entities
{

    public class DateViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public int? Day { get; set; }
        public string? Weekday { get; set; } = string.Empty; // Store weekdays as JSON
        public string? Month { get; set; } = string.Empty; // Store months as JSON
        public int? Year { get; set; } = 0;
        [EditableForChapter]
        public int? EventId { get; set; }
        public string? EventName { get; set; } = string.Empty;// Readable Name
    }

}
