using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Models;

namespace FantasyDB.ViewModels
{
    public class FactionViewModel
    {
        public string? Name { get; set; } = string.Empty;
        public string? Alias { get; set; } = string.Empty;
        public int? FoundingYear { get; set; }
        public string? Magic { get; set; } = string.Empty;
        [NotMapped]
        public string? FounderName { get; set; } = string.Empty;// Readable Name
        [NotMapped]
        public string? LeaderName { get; set; } = string.Empty;// Readable Name
        [NotMapped]
        public string? HQLocationName { get; set; } = string.Empty;
        [NotMapped]
        public string? SnapshotName { get; set; } = string.Empty;
    }
}
