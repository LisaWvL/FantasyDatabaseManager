using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasyDB.Models;

namespace FantasyDB.ViewModels
{
    public class CharacterViewModel
    {
        public string? Role { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public string? Alias { get; set; } = string.Empty;
        public int? BirthDay { get; set; }  
        public string? BirthMonth { get; set; } = string.Empty;
        public int? BirthYear { get; set; } 
        public string? Gender { get; set; } = string.Empty;
        public int? HeightCm { get; set; }
        public string? Build { get; set; } = string.Empty;
        public string? Hair { get; set; } = string.Empty;
        public string? Eyes { get; set; } = string.Empty;
        public string? DefiningFeatures { get; set; } = string.Empty;
        public string? Personality { get; set; } = string.Empty;
        public string? SocialStatus { get; set; } = string.Empty;
        public string? Occupation { get; set; } = string.Empty;
        public string? Magic { get; set; } = string.Empty;
        public string? Desire { get; set; } = string.Empty;
        public string? Fear { get; set; } = string.Empty;
        public string? Weakness { get; set; } = string.Empty;
        public string? Motivation { get; set; } = string.Empty;
        public string? Flaw { get; set; } = string.Empty;
        public string? Misbelief { get; set; } = string.Empty;
        public string? FactionName { get; set; } = string.Empty;// Readable Name
        public string? LocationName { get; set; } = string.Empty;// Readable Name
        public string? LanguageName { get; set; } = string.Empty;// Readable Name
        public string? SnapshotName { get; set; } = string.Empty;//Readable Name
    }
}
