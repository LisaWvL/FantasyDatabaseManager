using FantasyDB.Attributes;
using FantasyDB.Interfaces;

namespace FantasyDB.ViewModels
{
    public class CalendarViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public int? Day { get; set; }
        public string Weekday { get; set; } = string.Empty; // Store weekdays as JSON
        public string Month { get; set; } = string.Empty; // Store months as JSON
        [EditableForSnapshot]
        public int? EventId { get; set; }
        public string EventName { get; set; } = string.Empty;// Readable Name
    }
}
