using System.Diagnostics.CodeAnalysis;
using System;
using System.Collections.Generic;  // Needed for ICollection, List, and Dictionary
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FantasyDB.Models // ✅ Add this line
{

    public class Faction
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public int? FounderId { get; set; }
        public int? LeaderId { get; set; }
        public int? FoundingYear { get; set; }
        public int? HQLocationId { get; set; }
        public string? Magic { get; set; }
        public int? SnapshotId { get; set; }
        // ✅ Navigation Properties (Add These Back)
        [ForeignKey("FounderId")]
        public virtual Character? Founder { get; set; }
        [ForeignKey("LeaderId")]
        public virtual Character? Leader { get; set; }
        [ForeignKey("HQLocationId")]
        public virtual Location? HQLocation { get; set; }
        [ForeignKey("SnapshotId")]
        public virtual Snapshot? Snapshot { get; set; }

    }
}
