using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Models;

namespace FantasyDB.ViewModels
{
    public class PriceExampleViewModel
    {
        public string? Category { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public string? Exclusivity { get; set; } = string.Empty;
        public decimal? Price { get; set; }
    }
}
