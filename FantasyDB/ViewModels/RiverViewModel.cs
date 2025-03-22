using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Models;

namespace FantasyDB.ViewModels
{
    public class RiverViewModel
    {
        public string? Name { get; set; } = string.Empty;
        public int? DepthMeters { get; set; }
        public int? WidthMeters { get; set; }
        public string? FlowDirection { get; set; } = string.Empty;
        [NotMapped]
        public string? SourceLocationName { get; set; } = string.Empty;
        [NotMapped]
        public string? DestinationLocationName { get; set; } = string.Empty;
    }
}
