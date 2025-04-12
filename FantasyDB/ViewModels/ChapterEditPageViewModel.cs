using System.Collections.Generic;

namespace FantasyDB.ViewModels
{
    public class ChapterEditPageViewModel<TViewModel>
    {
        public TViewModel NewChapter { get; set; } = default!;
        public List<TViewModel> ExistingVersions { get; set; } = default!;
    }
}
