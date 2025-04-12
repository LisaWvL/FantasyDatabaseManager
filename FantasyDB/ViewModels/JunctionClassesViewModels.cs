using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Interfaces;
using FantasyDB.Models;

namespace FantasyDB.ViewModels
{
    internal class JunctionClassesViewModels
    {
        public class LanguageLocationViewModel : IViewModelWithId
        {
            public int Id { get; set; } // This is the primary key for the junction table
            public int LanguageId { get; set; }
            public Language Language { get; set; } = default!;
            public int LocationId { get; set; }
            public Location Location { get; set; } = default!;
        }


        public class PlotPointRiverViewModel : IViewModelWithId
        {
            public int Id { get; set; } // This is the primary key for the junction table
            public int PlotPointId { get; set; }
            public PlotPoint PlotPoint { get; set; } = default!;
            public int RiverId { get; set; }
            public River River { get; set; } = default!;
        }


        public class PlotPointRouteViewModel : IViewModelWithId

        {
            public int Id { get; set; } // This is the primary key for the junction table
            public int PlotPointId { get; set; }
            public PlotPoint PlotPoint { get; set; } = default!;
            public int RiverId { get; set; }
            public Route Route { get; set; } = default!;
        }

    }
}
