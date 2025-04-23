using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using FantasyDB.Entities._Shared;
using FantasyDB.Entities;


namespace FantasyDB.Entities // ✅ Add this line
{
    public class Chapter
    {
        public int Id { get; set; }
        public int? ChapterNumber { get; set; } = 0;
        public string? ChapterTitle { get; set; } = string.Empty;
        public string? ChapterText { get; set; }
        public int? WordCount { get; set; } = 0;
        public string? ChapterSummary { get; set; }
        public string? ToDo { get; set; }

        public int? StartDateId { get; set; }
        public virtual Date? StartDate { get; set; } = null!;
        public int? EndDateId { get; set; }
        public virtual Date? EndDate { get; set; } = null!;

        public int? ActId { get; set; }
        public virtual Act? Act { get; set; } = null!;

        public int? POVCharacterId { get; set; }
        public virtual Character? POVCharacter { get; set; }

        public List<Scene>? Scenes { get; set; } = [];
        public ICollection<ChapterPlotPoint> ChapterPlotPoints { get; set; } = [];

    }
}