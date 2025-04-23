using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Attributes;
using FantasyDB.Entities;
using FantasyDB.Entities._Shared;


namespace FantasyDB.Entities
{
    public class PlotPointViewModel : IViewModelWithId
    {
        public int Id { get; set; }

        [EditableForChapter]
        public string Title { get; set; } = string.Empty;

        [EditableForChapter]
        public string? Description { get; set; }

        [EditableForChapter]
        public int? StartDateId { get; set; }

        [EditableForChapter]
        public int? EndDateId { get; set; }

        public string? StartDateName { get; set; }

        public string? EndDateName { get; set; }

        [EditableForChapter]
        public List<int>? ChapterIds { get; set; } = [];


        public List<string>? ChapterTitles { get; set; } = [];
        [HandlesJunction("ChapterPlotPoint", "ChapterId", "PlotPointId")]
        public List<int>? PlotPointIds { get; set; } = [];

        public List<string>? PlotPointTitles { get; set; } = [];


        // Junctions

        [HandlesJunction("PlotPointRiver", "PlotPointId", "RiverId")]
        public List<int>? RiverIds { get; set; } = [];

        public List<string>? RiverNames { get; set; } = [];

        [HandlesJunction("PlotPointRoute", "PlotPointId", "RouteId")]
        public List<int>? RouteIds { get; set; } = [];

        public List<string>? RouteNames { get; set; } = [];
    }

}