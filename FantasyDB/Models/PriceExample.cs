using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FantasyDB.Models // ✅ Add this line
{
    public class PriceExample
    {
        public int Id { get; set; }
        public string? Category { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Exclusivity { get; set; }
        public decimal? Price { get; set; }
    }
}