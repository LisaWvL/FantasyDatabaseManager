

namespace FantasyDB.Entities._Shared
{

    public class LanguageLocation
    {
        public int LanguageId { get; set; }
        public Language Language { get; set; } = default!;

        public int LocationId { get; set; }
        public Location Location { get; set; } = default!;
    }

    public class PlotPointRiver
    {
        public int PlotPointId { get; set; }
        public PlotPoint PlotPoint { get; set; } = default!;

        public int RiverId { get; set; }
        public River River { get; set; } = default!;
    }

    public class PlotPointRoute
    {
        public int PlotPointId { get; set; }
        public PlotPoint PlotPoint { get; set; } = default!;

        public int RouteId { get; set; }
        public Route Route { get; set; } = default!;
    }

    public class ChapterPlotPoint
    {
        public int ChapterId { get; set; }
        public Chapter Chapter { get; set; } = default!;
        public int PlotPointId { get; set; }
        public PlotPoint PlotPoint { get; set; } = default!;
    }
}