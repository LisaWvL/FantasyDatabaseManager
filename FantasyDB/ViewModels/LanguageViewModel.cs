using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Models;

namespace FantasyDB.ViewModels
{
    public class LanguageViewModel
    {
        public string? Type { get; set; } = string.Empty;
        public string? Text { get; set; } = string.Empty;
        // ✅ Computed Property for Display
        [NotMapped]
        public string? LanguageName { get; set; } = string.Empty;
    }
}
