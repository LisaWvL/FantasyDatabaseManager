using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FantasyDB.Models // ✅ Add this line
{

    public class Era
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public string? MagicSystem { get; set; }
        public string? MagicStatus { get; set; }
        public int? SnapshotId { get; set; }
        // ✅ Navigation Properties (Add These Back)
        [ForeignKey("SnapshotId")]
        public virtual Snapshot? Snapshot { get; set; }
    }
}

