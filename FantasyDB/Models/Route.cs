using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Collections.Generic;  // Needed for ICollection, List, and Dictionary
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FantasyDB.Models // ✅ Add this line
{
    public class Route
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Type { get; set; }
        public int? Length { get; set; }
        public string? Notes { get; set; }
        public string? TravelTime { get; set; }
        public int? FromId { get; set; }
        public int? ToId { get; set; }
        // ✅ Navigation Properties (Add These Back)
        [ForeignKey("FromId")]
        public virtual Location? From { get; set; }
        [ForeignKey("ToId")]
        public virtual Location? To { get; set; }

    }
}