using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Models;
using FantasyDB.Services;

namespace FantasyDB.ViewModels
{
    public class PriceExampleViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public string? Category { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public string? Exclusivity { get; set; } = string.Empty;
        public int? Price { get; set; }
    }
}
