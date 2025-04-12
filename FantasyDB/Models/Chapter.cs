using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static FantasyDB.Models.JunctionClasses;

namespace FantasyDB.Models // ✅ Add this line
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

        public int? ActId { get; set; }
        public virtual Act? Act { get; set; } = null!;

        public int? POVCharacterId { get; set; }
        public virtual Character? POVCharacter { get; set; }

        public virtual ICollection<Scene> Scenes { get; set; } = new List<Scene>();
    }


}