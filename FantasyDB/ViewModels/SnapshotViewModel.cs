using FantasyDB.Interfaces;

namespace FantasyDB.ViewModels
{
    public class SnapshotViewModel : IViewModelWithId
    {

        public int Id { get; set; }
        public string? Book { get; set; } = string.Empty;
        public string? Act { get; set; } = string.Empty;
        public string? Chapter { get; set; } = string.Empty;
        public string? SnapshotName { get; set; } = string.Empty; // ✅  loaded from DB
    }

}
