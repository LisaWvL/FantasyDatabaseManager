using System.Collections.Generic;
using FantasyDB.Entities._Shared;
using FantasyDB.Entities;


namespace FantasyDB.Entities
{
    public class ChapterEditPageViewModel<TViewModel>
    {
        public TViewModel NewChapter { get; set; } = default!;
        public List<TViewModel> ExistingVersions { get; set; } = default!;
    }
}
