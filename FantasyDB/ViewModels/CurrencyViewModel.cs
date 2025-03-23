using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Models;
using FantasyDB.Services;

namespace FantasyDB.ViewModels
{
    public class CurrencyViewModel : IViewModelWithId
    {
        public int Id { get; set; }
        public int? Crown { get; set; } = 1;
        public int? Shilling { get; set; } = 12;
        public int? Penny { get; set; } = 240;
    }
}
