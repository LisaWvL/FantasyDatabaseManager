using FantasyDB.Attributes;
using FantasyDB.Interfaces;

namespace FantasyDB.ViewModels
{
    public class EventViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        [EditableForSnapshot]
        public string? Description { get; set; } = string.Empty;
        [EditableForSnapshot]
        public int? Day { get; set; }
        [EditableForSnapshot]
        public string? Month { get; set; } = string.Empty;
        [EditableForSnapshot]
        public int? Year { get; set; }
        public string? Purpose { get; set; } = string.Empty;
        [EditableForSnapshot]
        public int? SnapshotId { get; set; }
        [EditableForSnapshot]
        public int? LocationId { get; set; }
        public string? SnapshotName { get; set; } = string.Empty;// Readable Name
        public string? LocationName { get; set; } = string.Empty;// Readable Name
    }
}
