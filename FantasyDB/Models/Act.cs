using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using static FantasyDB.Models.JunctionClasses;

namespace FantasyDB.Models // ✅ Add this line
{
    public class Act
    {
        public int Id { get; set; }
        public string? ActTitle { get; set; } = string.Empty;
        public int? ActNumber { get; set; } = 0;
        public string? ActSummary { get; set; } = string.Empty;
        public string? ActToDo { get; set; } = string.Empty;
        public int? ActWordCount { get; set; } = 0;

        public int? BookId { get; set; }
        public virtual Book? Book { get; set; } = null!;

        public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
    }

}