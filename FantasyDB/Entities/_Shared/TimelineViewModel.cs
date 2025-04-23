using System.Collections.Generic;


namespace FantasyDB.Entities._Shared
{
    public class TimelineData
    {
        public List<TimelineEventVM> PlotPoints { get; set; } = [];
        public List<TimelineEventVM> Chapters { get; set; } = [];
        public List<TimelineEventVM> Eras { get; set; } = [];
        public List<TimelineEventVM> Events { get; set; } = [];
        public List<TimelineEmptySpan> EmptySpans { get; set; } = [];
    }

    public class TimelineEventVM
    {
        public int Id { get; set; }
        public string EntityType { get; set; } = "";
        public string Name { get; set; } = "";
        public int? StartDateId { get; set; }
        public int? EndDateId { get; set; }
    }

    public class TimelineEmptySpan
    {
        public int StartId { get; set; }
        public int EndId { get; set; }
    }
}
