using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyDB.ViewModels
{
    public class SnapshotEditPageViewModel<TViewModel>
    {
        public TViewModel? NewSnapshot { get; set; }
        public List<TViewModel> ExistingVersions { get; set; } = new();
    }
}