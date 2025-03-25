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
        [EditableForSnapshot]
        public string? Magic { get; set; } = string.Empty;
        [EditableForSnapshot]
        public int? SnapshotId { get; set; }
        public int? FounderId { get; set; }
        [EditableForSnapshot]
        public int? LeaderId { get; set; }
        [EditableForSnapshot]
        public int? HQLocationId { get; set; }
        public string? FounderName { get; set; } = string.Empty;// Readable Name
        public string? LeaderName { get; set; } = string.Empty;// Readable Name
        public string? HQLocationName { get; set; } = string.Empty;
        public string? SnapshotName { get; set; } = string.Empty;
    }
}
