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
        [NotMapped]
        public virtual Character? Founder { get; set; }
        [NotMapped]
        public virtual Character? Leader { get; set; }
        [NotMapped]
        public virtual Location? HQLocation { get; set; }
        [NotMapped]
        public virtual Snapshot? Snapshot { get; set; }

    }
}
