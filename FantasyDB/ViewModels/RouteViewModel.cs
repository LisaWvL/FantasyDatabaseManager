using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Models;

namespace FantasyDB.ViewModels
{
    public class RouteViewModel
    {
        public string? Name { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty;
        public int? Length { get; set; }
        public string? Notes { get; set; } = string.Empty;
        public string? TravelTime { get; set; } = string.Empty;
        [NotMapped]
        public string? ToLocationName { get; set; } = string.Empty;
        [NotMapped]
        public string? FromLocationName { get; set; } = string.Empty;

    }
}
