using FantasyDB.Attributes;
using FantasyDB.Interfaces;

namespace FantasyDB.ViewModels
{
    public class ArtifactViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Origin { get; set; } = string.Empty;
        public string? Effects { get; set; } = string.Empty;
        [EditableForSnapshot]
        public int? SnapshotId { get; set; }
        [EditableForSnapshot]
        public int? OwnerId { get; set; }
        public string? OwnerName { get; set; } = string.Empty; // Readable Name
        public string? SnapshotName { get; set; } = string.Empty;
    }
}
