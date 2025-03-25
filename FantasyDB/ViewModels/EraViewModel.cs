using FantasyDB.Attributes;
using FantasyDB.Interfaces;

namespace FantasyDB.ViewModels
{
    public class EraViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        [EditableForSnapshot]
        public string? Description { get; set; } = string.Empty;
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        [EditableForSnapshot]
        public string? MagicSystem { get; set; } = string.Empty;
        [EditableForSnapshot]
        public string? MagicStatus { get; set; } = string.Empty;
        [EditableForSnapshot]
        public int? SnapshotId { get; set; }
        public string? SnapshotName { get; set; } = string.Empty;
    }
}
