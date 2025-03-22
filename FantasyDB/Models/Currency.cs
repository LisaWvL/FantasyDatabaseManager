using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;  // Needed for ICollection, List, and Dictionary
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FantasyDB.Models // ✅ Add this line
{

    public class Currency
    {
        public int Id { get; set; }
        public int? Crown { get; set; } = 1;
        public int? Shilling { get; set; } = 12;
        public int? Penny { get; set; } = 240;

    }
}
