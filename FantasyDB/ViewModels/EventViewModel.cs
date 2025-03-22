using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Models;

namespace FantasyDB.ViewModels
{
    public class EventViewModel
    {
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int? Day { get; set; }
        public string? Month { get; set; } = string.Empty;
        public int? Year { get; set; }
        public string? Purpose { get; set; } = string.Empty;
        [NotMapped]
        public string? SnapshotName { get; set; } = string.Empty;// Readable Name
        [NotMapped]
        public string? LocationName { get; set; } = string.Empty;// Readable Name
    }
}
