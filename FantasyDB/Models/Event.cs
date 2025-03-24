using System.Diagnostics.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace FantasyDB.Models // ✅ Add this line
{

    public class Event
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? Day { get; set; }
        public string? Month { get; set; }
        public int? Year { get; set; }
        public string? Purpose { get; set; }
        public int? SnapshotId { get; set; }
        public int? LocationId { get; set; }
        // Navigation Properties
        [ForeignKey("LocationId")]
        public virtual Location? Location { get; set; }
        [ForeignKey("SnapshotId")]
        public virtual Snapshot? Snapshot { get; set; }
    }
}

