using System.Collections.Generic;

namespace FantasyDB.ViewModels
{
    public class SnapshotEditPageViewModel<TViewModel>
    {
        public TViewModel NewSnapshot { get; set; }
        public List<TViewModel> ExistingVersions { get; set; }
    }
}
