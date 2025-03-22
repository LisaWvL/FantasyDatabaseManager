using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Models;

namespace FantasyDB.ViewModels
{
    public class EraViewModel
    {
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public string? MagicSystem { get; set; } = string.Empty;
        public string? MagicStatus { get; set; } = string.Empty;
        [NotMapped]
        public string? SnapshotName { get; set; } = string.Empty;
    }
}
